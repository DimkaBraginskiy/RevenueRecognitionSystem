namespace RevenueRecognitionSystem.Services;

public interface IRevenueService
{
    Task<decimal> GetCurrentRevenueAsync(string? currency, CancellationToken token);
    Task<decimal> GetPredictedRevenueAsync(string? currency, CancellationToken token);
}