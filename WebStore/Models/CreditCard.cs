using System.ComponentModel.DataAnnotations;

namespace WebStore.Models;

public class CreditCard
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    [MaxLength (4)] // Max 4 tecken
    public string CardNumberLast4 { get; set; } = null!;
    
    public DateTime ExpirationDate { get; set; }
    public string CardType { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}