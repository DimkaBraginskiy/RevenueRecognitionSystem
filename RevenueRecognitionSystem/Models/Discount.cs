using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Discount")]
public class Discount
{
    [Key]
    public int IdDiscount { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string DiscountType { get; set; } = null!;

    [Required]
    [Range(1,100)]
    public decimal Value { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}