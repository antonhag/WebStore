namespace WebStore.Models;

public class Customer
{
    // PK
    public int Id { get; set; }
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Street { get; set; } 
    public string ZipCode { get; set; } 
    
    // FK till City
    public int CityId { get; set; }
    
    // Nav-property till City
    public virtual City City { get; set; }
    
    // Nav-property, en kund "kan ha" flera kreditkort, dock inte möjligt att lägga till fler kort i applikationen
    public virtual List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    
    // Nav-property, en kund kan ha flera ordrar
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();



}