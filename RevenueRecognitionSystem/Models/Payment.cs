using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Payment")]
public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdContract { get; set; }
    [ForeignKey(nameof(IdContract))]
    public Contract Contract { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime PaymentDate { get; set; }


}