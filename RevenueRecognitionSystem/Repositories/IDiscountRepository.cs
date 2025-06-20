using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public interface IDiscountRepository
{
    public Task<List<Discount>> GetActiveDiscountsAsync(CancellationToken token, DateTime onDate);
}