using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

public class Individual : Client
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Pesel { get; set; } = null!;
    [Required]
    public bool IsDeleted { get; set; } = false;
}