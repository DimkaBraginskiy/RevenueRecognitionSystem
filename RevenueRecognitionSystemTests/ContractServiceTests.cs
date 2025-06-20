using System.ComponentModel.DataAnnotations;
using Moq;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Services;
using RevenueRecognitionSystem.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace RevenueRecognitionSystemTests;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _contractRepoMock = new();
    private readonly Mock<IClientRepository> _clientsRepoMock = new();
    private readonly Mock<ISoftwareRepository> _softwareRepoMock = new();
    private readonly Mock<IDiscountRepository> _discountRepoMock = new();
    private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
    private readonly ContractService _service;

    public ContractServiceTests()
    {
        _service = new ContractService(
            _contractRepoMock.Object,
            _clientsRepoMock.Object,
            _softwareRepoMock.Object,
            _discountRepoMock.Object,
            _paymentRepoMock.Object
        );
    }

 
    [Fact]
    public async Task PayForContractAsync_Throws_WhenContractIsCancelled()
    {
        // Arrange
        var contract = new Contract
        {
            IdContract = 1,
            IsCancelled = true,
            EndDate = DateTime.UtcNow.AddDays(5) // Add this to prevent early expiration check from failing test
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        var dto = new PaymentRequestDto { ContractId = 1, Amount = 1000 };

        // Act + Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _service.PayForContractAsync(dto, CancellationToken.None));

        Assert.Equal("Contract is cancelled", ex.Message);
    }
    
    [Fact]
    public async Task PayForContractAsync_Throws_WhenContractIsAlreadySigned()
    {
        var contract = new Contract
        {
            IdContract = 2,
            IsCancelled = false,
            IsSigned = true,
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        var dto = new PaymentRequestDto { ContractId = 2, Amount = 500 };

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _service.PayForContractAsync(dto, CancellationToken.None));

        Assert.Equal("Contract already paid and signed", ex.Message);
    }
    
    [Fact]
    public async Task PayForContractAsync_Throws_WhenContractExpired()
    {
        var contract = new Contract
        {
            IdContract = 3,
            EndDate = DateTime.UtcNow.AddDays(-1)
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        var dto = new PaymentRequestDto { ContractId = 3, Amount = 1000 };

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _service.PayForContractAsync(dto, CancellationToken.None));

        Assert.Equal("Cannot pay after contract expiration", ex.Message);
    }
    
    [Fact]
    public async Task PayForContractAsync_Throws_WhenPaymentExceedsContractPrice()
    {
        var contract = new Contract
        {
            IdContract = 4,
            TotalPrice = 2000,
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(4, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        _paymentRepoMock.Setup(p => p.GetTotalPaidForContractAsync(4, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1500);

        var dto = new PaymentRequestDto { ContractId = 4, Amount = 600 };

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _service.PayForContractAsync(dto, CancellationToken.None));

        Assert.Equal("Payment exceeds contract price. Already paid: 1500.", ex.Message);
    }
    
    [Fact]
    public async Task PayForContractAsync_SignsContract_WhenFullPaymentReached()
    {
        var contract = new Contract
        {
            IdContract = 5,
            TotalPrice = 1000,
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        _paymentRepoMock.Setup(p => p.GetTotalPaidForContractAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0) // No prior payment
            .Callback(() =>
            {
                // After payment, simulate full payment total
                _paymentRepoMock.Setup(p => p.GetTotalPaidForContractAsync(5, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1000);
            });

        var dto = new PaymentRequestDto { ContractId = 5, Amount = 1000 };

        await _service.PayForContractAsync(dto, CancellationToken.None);

        _contractRepoMock.Verify(r => r.UpdateContractAsync(
            It.Is<Contract>(c => c.IsSigned == true), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task PayForContractAsync_AddsPayment_WhenPartialPayment()
    {
        var contract = new Contract
        {
            IdContract = 6,
            TotalPrice = 3000,
            EndDate = DateTime.UtcNow.AddDays(5)
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contract);

        _paymentRepoMock.Setup(p => p.GetTotalPaidForContractAsync(6, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1000);

        var dto = new PaymentRequestDto { ContractId = 6, Amount = 500 };

        await _service.PayForContractAsync(dto, CancellationToken.None);

        _paymentRepoMock.Verify(p => p.AddPaymentAsync(
                It.Is<Payment>(p => p.Amount == 500 && p.IdContract == 6), It.IsAny<CancellationToken>()),
            Times.Once);

        _contractRepoMock.Verify(r => r.UpdateContractAsync(It.IsAny<Contract>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}