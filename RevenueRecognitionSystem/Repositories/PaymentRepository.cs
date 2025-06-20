﻿using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    
    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<decimal> GetTotalPaidForContractAsync(int contractId, CancellationToken token)
    {
        return await _context.Payments
            .Where(p => p.IdContract == contractId)
            .SumAsync(p => p.Amount, token);
    }
    
    public async Task AddPaymentAsync(Payment payment, CancellationToken token)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task<decimal> GetTotalConfirmedRevenueAsync(CancellationToken token)
    {
        return await _context.Payments
            .Where(p => p.Contract.IsSigned && !p.Contract.IsCancelled)
            .SumAsync(p => p.Amount, token);
    }

   
}