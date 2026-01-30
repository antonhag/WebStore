using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Data;

public class WebStoreContext : DbContext
{
    private readonly string _connectionString = ConnectionStringHelper.GetSqlConnectionString();
    
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
    public DbSet<DeliveryOption> DeliveryOptions { get; set; } = null!;
    public DbSet<CreditCard> CreditCards { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;

    
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("webstore");
        
        // Kund email måste vara unik
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();
        
        // Ser till så att det inte går att radera en kategori med produkter i.
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Ser till så att det inte går att radera en ett land med städer i.
        modelBuilder.Entity<Country>()
            .HasMany(c => c.Cities)
            .WithOne(c => c.Country)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.City)
            .WithMany(c => c.Customers)
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Ser till att standardvärde för StockQuantity blir 0 istället för null
        modelBuilder.Entity<Product>()
            .Property(p => p.StockQuantity)
            .HasDefaultValue(0);
    }
}