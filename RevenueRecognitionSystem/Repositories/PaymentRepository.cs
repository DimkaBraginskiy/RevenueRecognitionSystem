using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class PaymentRepository
{
    private readonly AppDbContext _context;
    
    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<decimal> GetTotalPaidForContractAsync(int contractId, CancellationToken token)
    {
        return await _context.Payments
            .Where(p => p.Id == contractId)
            .SumAsync(p => p.Amount, token);
    }
    
    public async Task AddPaymentAsync(Payment payment, CancellationToken token)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(token);
    }
}