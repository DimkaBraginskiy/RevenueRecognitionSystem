using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public interface ISoftwareRepository
{
    public Task<Software?> GetSoftwareByIdAsync(CancellationToken token, int softwareId);
}