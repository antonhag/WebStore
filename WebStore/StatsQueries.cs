using System.Runtime.InteropServices.JavaScript;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;

namespace WebStore;

public class StatsQueries
{
    // Asynkron metod som hämtar statistik om bästa kunder per region (baserat på antal ordrar)
    public static async Task<Dictionary<string, int>> GetBestSwedishCustomerPerRegionAsync(string connectionString)
    {
        var result = new Dictionary<string, int>(); // string = regionens namn (nyckel), int = antal ordrar (värde)

        using (var connection = new SqlConnection(connectionString))
        {
            var sql = new SqlCommand(@"
                            SELECT c.Region, COUNT(o.Id) AS OrderCount 
                            FROM webstore.Orders o
                            INNER JOIN WebStore.Customers cu ON o.CustomerId = cu.Id
                            INNER JOIN webstore.Cities c ON cu.CityId = c.Id
                            WHERE c.Region IS NOT NULL -- Ta bort null värden eftersom de andra ländernas städer inte har något region värde
                            GROUP BY c.Region", connection);

            // öppnar databasanslutningen asynkront
            await connection.OpenAsync();

            // Kör queryn och får tillbaka en SqlDataReader
            using (var reader = await sql.ExecuteReaderAsync())
            {
                // Läser resultatet rad för rad sålänge det finns data
                while (await reader.ReadAsync())
                {
                    string region = reader.GetString(0); // Hämtar
                    int orders = reader.GetInt32(1);
                    result.Add(region, orders);
                }
            }
        }

        return result;
    }

    public static async Task<Dictionary<string, decimal>> Get5BestSpendingCustomersAsync()
    {
        using var db = new WebStoreContext();

        // kör query asynkront
        var query = await db.Orders
            .GroupBy(o => new
            {
                o.Customer.FirstName,
                o.Customer.LastName,
            })
            .Select(g => new
            {
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToListAsync();

        // Konvertera till dictionary, så returtypen blir korrekt
        var result = query.ToDictionary(
            x => $"{x.FirstName} {x.LastName}", // nyckel = namn
            x => x.TotalSpent); // värde = totalt spenderat

        return result;
    }

    public static async Task<Dictionary<string, Dictionary<string, int>>> 
        GetPopularProductsByAgeAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var sql = @"
            SELECT
                c.FirstName,
                c.LastName,
                c.BirthDate,
                p.Name AS ProductName,
                oi.Quantity
            FROM webstore.OrderItems oi
            INNER JOIN webstore.Orders o ON oi.OrderId = o.Id
            INNER JOIN webstore.Customers c ON o.customerId = c.Id
            INNER JOIN webstore.Products p ON oi.ProductId = p.Id
";
        
        var rows = await connection.QueryAsync(sql);
        
        // Hämtar dagens datum för att sedan beräkna ålder
        var today = DateTime.Today;
        
        // Skapar dictionaryn som ska returneras
        // Yttersta dictionaryn: nyckel = åldersgrupp, värde = dictionary med produkt och antal
        var result = new Dictionary<string, Dictionary<string, int>>();

        // Loopar igenom varje rad som hämtats från databasen
        foreach (var row in rows)
        {
            // Beräkna kundens ålder
            DateTime birthDate = row.BirthDate;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            // dela in kunderna i två olika åldersgrupper
            string ageGroup = age < 30 ? "Under 30" : "30 och äldre";
            
            string product = row.ProductName;
            int quantity = row.Quantity;

            // Ifall åldersgruppen inte finns i dictionaryn, ska en ny inre dictionary
            if (!result.ContainsKey(ageGroup))
            {
                result[ageGroup] = new Dictionary<string, int>();
            }

            // Ifall produkten inte finns i den inre dictionaryn, sätt startvärde 0
            if (!result[ageGroup].ContainsKey(product))
            {
                result[ageGroup][product] = 0;
            }
            
            // Lägg till kvantiteten för produkten i den inre dictionaryn
            result[ageGroup][product] += quantity;
        }

        return result;
    }

    public static async Task<Dictionary<string, int>> GetCustomersCountPerCityAsync()
    {
        using var db = new WebStoreContext();

        var query = await db.Customers
            .Include(c => c.City)
            .GroupBy(c => c.City != null ? c.City.Name : "Ingen stad angiven") // Hantera null eftersom jag har tillåtit att ej behöva ange stad.
            .Select(g => new
            {
                CityName = g.Key,
                CustomerCount = g.Count()
            })
            .OrderByDescending(x => x.CustomerCount)
            .ToListAsync();
        
        return query.ToDictionary(x=> x.CityName, x=> x.CustomerCount);
    }

    public static async Task<Dictionary<string, int>> Get5MostPopularProductAsync()
    {
        using var db = new WebStoreContext();

        var query = await db.OrderItems
            .GroupBy(oi => oi.Product.Name)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalQuantity = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(5)
            .ToListAsync();
        
        return query.ToDictionary(x => x.ProductName, x => x.TotalQuantity); // nyckel = produktnamn, värde = antal sålda
    }

    public static async Task<Dictionary<string, int>> GetSalesBySupplierAsync()
    {
        using var db = new WebStoreContext();

        var query = await db.OrderItems
            .GroupBy(oi => oi.Product.Supplier)
            .Select(g => new
            {
                SupplierName = g.Key,
                TotalQuantity = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToListAsync();
        
        return query.ToDictionary(x => x.SupplierName, x => x.TotalQuantity);
    }
}