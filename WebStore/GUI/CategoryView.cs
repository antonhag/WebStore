using Dapper;
using Microsoft.Data.SqlClient;
using WebStore.Controllers;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CategoryView
{
    public static void Show(List<string> categories)
    {
        categories.Add("S. För att söka efter produkter");
        categories.Add("9. För att gå tillbaka till menyn");
        var categoryWindow = new Window ("Kategorier", 2, 10, categories);
        
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

    public static async Task ShowSearchResultsAsync(List<Product> products)
    {
        Console.Clear();
        await HeaderView.ShowWithDealsAsync();
        
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

    public static async Task SearchError(string message)
    {
        Console.Clear();
        await HeaderView.ShowWithDealsAsync();
        
        var errorWindow = new Window ("Fel", 2, 10, new List<string> { message, "Tryck valfri knapp för att gå tillbaka" });
        errorWindow.Draw();
    }
}