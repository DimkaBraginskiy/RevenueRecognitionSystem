namespace RevenueRecognitionSystem.DTOs.Response;

public class ClientResponseDto
{
    public string ClientType { get; set; }
    
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    // Individual fields
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Pesel { get; set; }

    // Company fields
    public string? CompanyName { get; set; }
    public string? Krs { get; set; }
}