namespace WebStore.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}