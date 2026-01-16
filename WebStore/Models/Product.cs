namespace WebStore.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Supplier { get; set; }
    public bool SelectedProduct { get; set; }
    public int CategoryId { get; set; }
    public int StockQuantity { get; set; } = 0;
    
    // Nav property
    public virtual Category Category { get; set; } = null!;
}