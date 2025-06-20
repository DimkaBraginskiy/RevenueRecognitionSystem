using System.ComponentModel.DataAnnotations;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Repositories;

namespace RevenueRecognitionSystem.Services;

public class ContractService : IContractService
{
    private readonly ContractRepository _contractRepository;
    private readonly ClientsRepository _clientsRepository;
    private readonly SoftwareRepository _softwareRepository;
    private readonly DiscountRepository _discountRepository;

    public ContractService(ContractRepository contractRepository, ClientsRepository clientsRepository, SoftwareRepository softwareRepository, DiscountRepository discountRepository)
    {
        _contractRepository = contractRepository;
        _clientsRepository = clientsRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
    }

    public async Task CreateContractAsync(CancellationToken token, ContractRequestDto dto)
    {
        if (dto.StartDate >= dto.EndDate)
        throw new ValidationException("End date must be after start date.");

    var days = (dto.EndDate - dto.StartDate).TotalDays;
    if (days < 3 || days > 30)
        throw new ValidationException("Contract duration must be between 3 and 30 days.");

    var client = await _clientsRepository.GetClientByIdAsync(token, dto.ClientId)
        ?? throw new NotFoundException($"Client with id {dto.ClientId} not found.");

    var software = await _softwareRepository.GetSoftwareByIdAsync(token, dto.SoftwareId)
        ?? throw new NotFoundException($"Software with id {dto.SoftwareId} not found.");

    if (await _contractRepository.HasActiveContractOrSubscriptionAsync(token, dto.ClientId, dto.SoftwareId))
        throw new ConflictException("Client already has an active contract or subscription for this software.");

    var basePrice = software.BaseYearlyPrice;

    var discounts = await _discountRepository.GetActiveDiscountsAsync(token, dto.StartDate);
    var maxDiscount = discounts.Any() ? discounts.Max(d => d.Value) : 0;

    if (await _contractRepository.IsReturningClientAsync(token, dto.ClientId))
        maxDiscount += 5;

    if (maxDiscount > 100)
        maxDiscount = 100;

    decimal discountMultiplier = (100 - maxDiscount) / 100m;
    decimal supportCost = dto.ExtraSupportYears * 1000m;

    if (dto.ExtraSupportYears < 0 || dto.ExtraSupportYears > 3)
        throw new ValidationException("Extra support must be between 0 and 3 years.");

    decimal finalPrice = Math.Round((basePrice + supportCost) * discountMultiplier, 2);

    var contract = new Contract
    {
        IdClient = client.Id,
        IdSoftware = software.IdSoftware,
        SoftwareVersionAtPurchase = dto.SoftwareVersionAtPurchase,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        ExtraSupportYears = dto.ExtraSupportYears,
        TotalPrice = finalPrice,
        IsCancelled = false,
        IsSigned = false,
        CreatedAt = DateTime.UtcNow,
    };

    await _contractRepository.AddContractAsync(token, contract);
    }
}