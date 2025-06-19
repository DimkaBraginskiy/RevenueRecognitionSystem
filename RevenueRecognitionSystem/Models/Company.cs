using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

public class Company : Client
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Krs { get; set; } = null!;
}