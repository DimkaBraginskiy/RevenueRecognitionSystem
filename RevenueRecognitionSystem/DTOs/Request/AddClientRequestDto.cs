namespace RevenueRecognitionSystem.DTOs;

public class AddClientRequestDto
{
    public string ClientType { get; set; }
    
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Pesel { get; set; }
    
    public string? Name { get; set; }
    public string? Krs { get; set; }
}