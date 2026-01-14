namespace WebStore.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CountryId { get; set; }
    public virtual Country Country { get; set; } = null!;
}