using RevenueRecognitionSystem.DTOs;

namespace RevenueRecognitionSystem.Services;

public interface IContractService
{
    Task CreateContractAsync(CancellationToken token, ContractRequestDto dto);
}