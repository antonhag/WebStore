using System.Diagnostics;
using Microsoft.Data.SqlClient;
using WebStore.GUI;

namespace WebStore.Controllers;

public class StatsController : ControllerBase
{
    protected override async Task DrawViewAsync()
    {
        StatsView.ShowMenu();
    }

    protected override async Task<bool> HandleInputAsync()
    {
        var key = Console.ReadKey(true).KeyChar;

        switch (key)
        {
            case '1':
                // Visa bäst kunder per region
                
                // HandleInput kan ej vara async (ärver från ControllerBase), därför används GetAwaiter().GetResult() för att köra en asynkron
                // metod synkront och vänta in resultatet innan vi fortsätter
                await ShowBestSwedishCustomersPerRegionAsync();
                return true;
            case '2':
                // Visa kunder som spenderat mest
                await Show5BestSpendingCustomersAsync();
                return true;
            case '3':
                // Visa populäraste produkter per åldersgrupp
                await ShowPopularProductsByAgeAsync();
                return true;
            case '4':
                // Visa antal kunder per stad
                await ShowCustomersCountPerCityAsync();
                return true;
            case '5':
                // Visa 5 mest populära produkter
                await Show5MostPopularProductAsync();
                return true;
            case '6':
                // Visa försäljning per leverantör
                await ShowSalesBySupplierAsync();
                return true;
            case '7':
                await StatsView.ShowSearchHistoryView();
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
            "Server=tcp:webstoredb.database.windows.net,1433;Initial Catalog=WebStoreDb;Persist Security Info=False;User ID=dbadmin;Password=Molle123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                
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
            "Server=tcp:webstoredb.database.windows.net,1433;Initial Catalog=WebStoreDb;Persist Security Info=False;User ID=dbadmin;Password=Molle123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        var sw = Stopwatch.StartNew();
        
        
        
        Console.WriteLine("Hämtar data...");
        
        var result = await StatsQueries.GetPopularProductsByAgeAsync(connectionString);
        
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