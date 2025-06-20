using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Repositories;

public interface IClientRepository
{
    public Task AddClientAsync(CancellationToken token, Individual individual);
    public Task AddClientAsync(CancellationToken token, Company company);
    public Task<IEnumerable<Individual>> GetAllIndividualsAsync(CancellationToken token);
    public Task<IEnumerable<Company>> GetAllCompaniesAsync(CancellationToken token);
    public Task<Client?> GetClientByIdAsync(CancellationToken token, int id);
    public Task UpdateClientAsync(CancellationToken token, Client client);
    public Task DeleteClientAsync(CancellationToken token, int id);

}