using System.Diagnostics;
using Microsoft.Data.SqlClient;
using WebStore.GUI;

namespace WebStore.Controllers;

public class StatsController : ControllerBase
{
    protected override void DrawView()
    {
        StatsView.ShowMenu();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).KeyChar;

        switch (key)
        {
            case '1':
                // Visa bäst kunder per region
                
                // HandleInput kan ej vara async (ärver från ControllerBase), därför används GetAwaiter().GetResult() för att köra en asynkron
                // metod synkront och vänta in resultatet innan vi fortsätter
                ShowBestSwedishCustomersPerRegionAsync().GetAwaiter().GetResult(); 
                return true;
            case '2':
                // Visa kunder som spenderat mest
                Show5BestSpendingCustomersAsync().GetAwaiter().GetResult();
                return true;
            case '3':
                // Visa populäraste produkter per åldersgrupp
                ShowPopularProductsByAgeAsync().GetAwaiter().GetResult();
                return true;
            case '4':
                // Visa antal kunder per stad
                ShowCustomersCountPerCityAsync().GetAwaiter().GetResult();
                return true;
            case '5':
                // Visa 5 mest populära produkter
                Show5MostPopularProductAsync().GetAwaiter().GetResult();
                return true;
            case '6':
                // Visa försäljning per leverantör
                ShowSalesBySupplierAsync().GetAwaiter().GetResult();
                return true;
            case '9':
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }
    
    private async Task ShowBestSwedishCustomersPerRegionAsync()
    {
        Console.Clear();
        Console.WriteLine("Bäst kunder per region");

        string connectionString =
            "Server=localhost,14330;Database=WebStoreDb;User Id=sa;Password=StrongP@ssw0rd!;TrustServerCertificate=True;";
                
        var sw = Stopwatch.StartNew();

        Task<Dictionary<string, int>> task = StatsQueries.GetBestSwedishCustomerPerRegionAsync(connectionString);

        Console.WriteLine("Hämtar data...");
        
        var result = await task;
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.ShowBestSwedishCustomerPerRegionView(result);
    }

    private async Task Show5BestSpendingCustomersAsync()
    {
        Console.Clear();
        Console.WriteLine("Topp 5 kunder som spenderat mest\n");
        
        var sw = Stopwatch.StartNew();

        Console.WriteLine("Hämtar data...");
        
        var result = await StatsQueries.Get5BestSpendingCustomersAsync();
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.Show5BestSpendingCustomersView(result);
    }

    private async Task ShowPopularProductsByAgeAsync()
    {
        Console.Clear();
        Console.WriteLine("Populäraste produkter av ålder\n");
        
        string connectionString =
            "Server=localhost,14330;Database=WebStoreDb;User Id=sa;Password=StrongP@ssw0rd!;TrustServerCertificate=True;";
        
        var sw = Stopwatch.StartNew();
        
        Task<Dictionary<string, Dictionary<string, int>>> task = StatsQueries.GetPopularProductsByAgeAsync(connectionString);
        
        Console.WriteLine("Hämtar data...");
        
        var result = await task;
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.ShowPopularProductsByAgeView(result);
    }

    private async Task ShowCustomersCountPerCityAsync()
    {
        Console.Clear();
        Console.WriteLine("Antal kunder per stad\n");
        
        var sw = Stopwatch.StartNew();

        Console.WriteLine("Hämtar data...");
        
        var result = await StatsQueries.GetCustomersCountPerCityAsync();
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.ShowCustomersCountPerCityView(result);
    }

    private async Task Show5MostPopularProductAsync()
    {
        Console.Clear();
        Console.WriteLine("Topp 5 mest populära produkter\n");
        
        var sw = Stopwatch.StartNew();

        Console.WriteLine("Hämtar data...");
        
        var result = await StatsQueries.Get5MostPopularProductAsync();
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.Show5MostPopularProductView(result);
    }

    private async Task ShowSalesBySupplierAsync()
    {
        Console.Clear();
        Console.WriteLine("Visar sålda produkter per leverantör");
        
        var sw = Stopwatch.StartNew();

        Console.WriteLine("Hämtar data...");
        
        var result = await StatsQueries.GetSalesBySupplierAsync();
        
        sw.Stop();
        
        Console.WriteLine($"Queryn tog {sw.ElapsedMilliseconds} ms\n");
        StatsView.ShowSalesBySupplierView(result);
    }
}