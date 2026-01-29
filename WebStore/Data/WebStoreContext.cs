using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data;

public class WebStoreContext : DbContext
{
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
        // optionsBuilder.UseSqlServer("Server=localhost,14330;Database=WebStoreDb;User Id=sa;Password=StrongP@ssw0rd!;TrustServerCertificate=True;");
        
        optionsBuilder.UseSqlServer("Server=tcp:webstoredb.database.windows.net,1433;Initial Catalog=WebStoreDb;Persist Security Info=False;User ID=dbadmin;Password=Molle123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("webstore");
    }
}