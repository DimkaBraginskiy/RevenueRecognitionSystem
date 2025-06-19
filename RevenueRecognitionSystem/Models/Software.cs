using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Software")]
public class Software
{
    [Key]
    public int IdSoftware { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string CurrentVersion { get; set; } = null!;
    [Required]
    public string Category { get; set; } = null!;
    
    
}