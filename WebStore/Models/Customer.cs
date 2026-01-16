namespace WebStore.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string? Street { get; set; } 
    
    public int? CityId { get; set; }
    public virtual City? City { get; set; }
    
    
    
}