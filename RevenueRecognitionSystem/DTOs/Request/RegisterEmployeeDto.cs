namespace RevenueRecognitionSystem.DTOs;

public class RegisterEmployeeDto
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}