using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public interface IPaymentRepository
{
    public Task<decimal> GetTotalPaidForContractAsync(int contractId, CancellationToken token);
    public Task AddPaymentAsync(Payment payment, CancellationToken token);
    public Task<decimal> GetTotalConfirmedRevenueAsync(CancellationToken token);
}