using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.DTOs.Response;

namespace RevenueRecognitionSystem.Services;

public interface IAuthService
{
    public Task RegisterEmployeeAsync(CancellationToken token, RegisterEmployeeDto dto);

    public Task<LoginResponseDto> LoginEmployeeAsync(CancellationToken token, LoginRequestDto dto);
}