using Dapper;
using Microsoft.Data.SqlClient;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CategoryView
{
    public static async Task ShowAsync()
    {
        var categoryList = new List<string>();
        
        var connectionString = "Server=localhost,14330;Database=WebStoreDb;User Id=sa;Password=StrongP@ssw0rd!;TrustServerCertificate=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "SELECT Id, Name FROM webstore.Categories"; // Dapper
            
            var categories = await connection.QueryAsync<Category>(sql);

            foreach (var category in categories)
            {
                categoryList.Add($"{category.Id}. {category.Name}");
            }
            categoryList.Add("S. För att söka efter produkt");
            categoryList.Add("9. För att gå tillbaka menyn");
        }
        var categoryWindow = new Window ("Kategorier", 2, 10, categoryList);
        
        categoryWindow.Draw();
    }

    public static string SearchView()
    {
        var searchWindow = new Window("Sök produkt", 2, 18, new List<string> { "Namn: " });
        searchWindow.Draw();
        
        Console.SetCursorPosition(10, 19);
        var searchedText = Console.ReadLine();
        return searchedText;
    }

    public static void ShowSearchResults(List<Product> products)
    {
        Console.Clear();
        HeaderView.ShowWithDeals();
        
        var rows = new List<string>();
        int index = 1;

        foreach (var product in products)
        {
            rows.Add($"{index}. {product.Name} - {product.Price} kr");
            index++;
        }
        rows.Add("9. För att gå tillbaka till menyn");
        
        var resultWindow = new Window ("Sökresultat", 2, 10, rows);
        resultWindow.Draw();
    }

    public static void SearchError(string message)
    {
        Console.Clear();
        HeaderView.ShowWithDeals();
        
        var errorWindow = new Window ("Fel", 2, 10, new List<string> { message, "Tryck valfri knapp för att gå tillbaka" });
        errorWindow.Draw();
    }
}