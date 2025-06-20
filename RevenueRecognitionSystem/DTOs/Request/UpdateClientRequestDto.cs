﻿namespace RevenueRecognitionSystem.DTOs;

public class UpdateClientRequestDto
{
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? CompanyName { get; set; }
}