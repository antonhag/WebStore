namespace WebStore.Models;

public class DeliveryOption
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string EstimatedTime { get; set; } = null!;
    public decimal Cost { get; set; } 
    public string Description { get; set; } = null!;
}