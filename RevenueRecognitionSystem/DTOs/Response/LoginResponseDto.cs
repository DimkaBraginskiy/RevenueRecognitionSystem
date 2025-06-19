namespace RevenueRecognitionSystem.DTOs.Response;

public class LoginResponseDto
{
    public string Login { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}