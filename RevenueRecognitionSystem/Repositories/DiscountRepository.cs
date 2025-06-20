using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class DiscountRepository
{
    private readonly AppDbContext _context;
    
    public DiscountRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Discount>> GetActiveDiscountsAsync(CancellationToken token, DateTime onDate)
    {
        return await _context.Discounts
            .Where(d => d.StartDate <= onDate && d.EndDate >= onDate)
            .ToListAsync(token);
    }
}