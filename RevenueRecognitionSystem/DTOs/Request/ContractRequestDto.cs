namespace RevenueRecognitionSystem.DTOs;

public class ContractRequestDto
{
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string SoftwareVersionAtPurchase { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ExtraSupportYears { get; set; }
}