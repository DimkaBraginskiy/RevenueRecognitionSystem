using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Company")]
public class Company : Client
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Krs { get; set; } = null!;
}