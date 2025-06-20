using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class ContractRepository
{
    private readonly AppDbContext _context;
    
    public ContractRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> HasActiveContractOrSubscriptionAsync(CancellationToken token, int clientId, int softwareId)
    {
        return await _context.Contracts
            .AnyAsync(c => c.IdClient == clientId && c.IdSoftware == softwareId && !c.IsCancelled && !c.IsSigned, token);
    }
    
    public async Task<bool> IsReturningClientAsync(CancellationToken token, int clientId)
    {
        return await _context.Contracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned, token);
    }
    
    public async Task AddContractAsync(CancellationToken token, Contract contract)
    {
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task<Contract?> GetContractByIdAsync(int id, CancellationToken token)
    {
        return await _context.Contracts.FindAsync(new object[] { id }, token);
    }

    public async Task UpdateContractAsync(Contract contract, CancellationToken token)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync(token);
    }

}