using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Employee")]
public class Employee
{
    [Key]
    public int IdEmployee { get; set; }
    
    [Required]
    public string Login { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    public string Salt { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
    
    
    public string RefreshToken { get; set; } = null!;
    public DateTime? RefreshTokenExp { get; set; }
}