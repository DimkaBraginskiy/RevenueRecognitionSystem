using RevenueRecognitionSystem.DTOs;

namespace RevenueRecognitionSystem.Services;

public interface IContractService
{
    Task CreateContractAsync(CancellationToken token, ContractRequestDto dto);
    public Task PayForContractAsync(PaymentRequestDto dto, CancellationToken token);
}