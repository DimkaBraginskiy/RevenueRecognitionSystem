using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem;


public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Individual> Individuals { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Software> Softwares { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;

    
    protected AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Client>()
            .HasDiscriminator<string>("ClientType")
            .HasValue<Individual>("Individual")
            .HasValue<Company>("Company");

        
        modelBuilder.Entity<Individual>()
            .HasIndex(i => i.Pesel)
            .IsUnique();

        modelBuilder.Entity<Company>()
            .HasIndex(c => c.Krs)
            .IsUnique();

        modelBuilder.Entity<Individual>()
            .Property(i => i.Pesel)
            .ValueGeneratedNever();

        modelBuilder.Entity<Company>()
            .Property(c => c.Krs)
            .ValueGeneratedNever();

        modelBuilder.Entity<Discount>()
            .Property(d => d.Value)
            .HasPrecision(10, 2);
        
        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                IdSoftware = 1,
                Name = "AccountingPro",
                Description = "Finance software",
                CurrentVersion = "1.0.0",
                Category = "Finance",
                BaseYearlyPrice = 5000m
            }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                IdDiscount = 1,
                DiscountType = "temporary",
                Name = "Black Friday",
                Value = 10,
                StartDate = new DateTime(2024, 11, 25),
                EndDate = new DateTime(2024, 12, 5)
            }
        );
    }
}