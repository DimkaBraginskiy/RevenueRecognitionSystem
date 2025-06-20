using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly AppDbContext _context;
    
    public SoftwareRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Software?> GetSoftwareByIdAsync(CancellationToken token, int softwareId)
    {
        return await _context.Softwares.FindAsync(new object[] { softwareId }, token);
    }
}