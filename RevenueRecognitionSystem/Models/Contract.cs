using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Contract")]
public class Contract
{
    [Key]
    public int IdContract { get; set; }

    [Required]
    public int IdClient { get; set; }
    
    [ForeignKey(nameof(IdClient))]
    public Client Client { get; set; } = null!;

    [Required]
    public int IdSoftware { get; set; }
    
    [ForeignKey(nameof(IdSoftware))]
    public Software Software { get; set; } = null!;
    
    [Required]
    public string SoftwareVersionAtPurchase { get; set; } = null!;


    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }
    [Range(0,3)]
    public int ExtraSupportYears { get; set; } = 0;
    [Required]
    public bool IsSigned { get; set; } = false;
    [Required]
    public bool IsCancelled { get; set; } = false;
    [Required]
    public DateTime CreatedAt { get; set; }
    
    
    public ICollection<Payment> Payments = new List<Payment>();
}