namespace WebStore.Models;

public class Order
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    public int PaymentMethodId { get; set; }
    public int DeliveryOptionId { get; set; }
    
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Klar";
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public virtual Customer Customer { get; set; } = null!;
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
    public virtual DeliveryOption DeliveryOption { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}