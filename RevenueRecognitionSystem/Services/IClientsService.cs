using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.DTOs.Response;

namespace RevenueRecognitionSystem.Services;

public interface IClientsService
{
    Task AddClientAsync(CancellationToken token, AddClientRequestDto dto);
    
    Task<List<ClientResponseDto>> GetClientsAsync(CancellationToken token);
    
    Task<ClientResponseDto> GetClientByIdAsync(CancellationToken token, int id);

    public Task UpdateClientAsync(CancellationToken token, int id, UpdateClientRequestDto dto);

    public Task DeleteClientAsync(CancellationToken token, int id);

}