using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public interface IContractRepository
{
    public Task<bool> HasActiveContractOrSubscriptionAsync(CancellationToken token, int clientId, int softwareId);
    public Task<bool> IsReturningClientAsync(CancellationToken token, int clientId);
    public Task AddContractAsync(CancellationToken token, Contract contract);
    public Task<Contract?> GetContractByIdAsync(int id, CancellationToken token);
    public Task UpdateContractAsync(Contract contract, CancellationToken token);
    public Task<decimal> GetUnsignedContractsTotalValueAsync(CancellationToken token);

}