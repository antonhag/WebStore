using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Controllers;

public class CategoryController : ControllerBase
{
    private List<(char key, Product product)> _dealsList;

    protected override async Task DrawViewAsync()
    {
        _dealsList = await HeaderView.ShowWithDealsAsync();

        var categories = await GetCategoriesAsync();
        CategoryView.Show(categories);
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            var key = Console.ReadKey(true).KeyChar;
            char upperKey = char.ToUpper(key);

            if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
            {
                await DealsHelper.HandleDealInput(_dealsList, upperKey);
                return true;
            }

            switch (upperKey)
            {
                case '1':
                    var productController1 = new ProductController(1);
                    await productController1.RunAsync();
                    return true;
                case '2':
                    var productController2 = new ProductController(2);
                    await productController2.RunAsync();
                    return true;
                case '3':
                    var productController3 = new ProductController(3);
                    await productController3.RunAsync();
                    return true;
                case '4':
                    var productController4 = new ProductController(4);
                    await productController4.RunAsync();
                    return true;
                case 'S':
                    await SearchProductAsync();
                    return true;
                case '9':
                    return false;
                default:
                    ShowError("Ogiltigt val!");
                    return true;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Något gick fel: {ex.Message}");
            return true;
        }
    }

    private async Task<List<string>> GetCategoriesAsync()
    {
        var categoriesString = new List<string>();
        
        var connectionString = ConnectionStringHelper.GetSqlConnectionString();
        using var connection = new SqlConnection(connectionString);

        var sql = "SELECT Id, Name FROM webstore.Categories"; // Dapper

        var categories = await connection.QueryAsync<Category>(sql);

        foreach (var category in categories)
        {
            categoriesString.Add($"{category.Id}. {category.Name}");
        }
        
        return categoriesString;
    }

    private async Task SearchProductAsync()
    {
        var searchedText = CategoryView.SearchView();

        using var db = new WebStoreContext();
        // Söker i databasen efter produkter vars namn innehåller det som användaren skrev (fritextsökning).
        var products = await db.Products.Where(p => EF.Functions.Like(p.Name, $"%{searchedText}%")).OrderBy(p => p.Name)
            .ToListAsync();

        await MongoLogger.LogProductSearchAsync(searchedText, Session.CurrentCustomer);

        if (products.Count < 1)
        {
            CategoryView.SearchError("Varan hittades ej");
            Console.ReadKey(true);
            return;
        }

        CategoryView.ShowSearchResultsAsync(products);
        await HandleSearchInputAsync(products);
    }

    private async Task HandleSearchInputAsync(List<Product> searchedProducts)
    {
        var key = Console.ReadKey(true).Key;
        int selectedIndex =
            (int)key - (int)ConsoleKey.D0; // D1 numeriska värde = 49. D1(49) - D0(48) = 1. D2(50) - D0(48) = 2 osv...

        if (selectedIndex > 0 && selectedIndex <= searchedProducts.Count)
        {
            int productId = searchedProducts[selectedIndex - 1].Id;

            await ProductListView.ShowDetailsAsync(productId);
            var key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.B)
            {
                await BuyProductAsync(productId);
            }
        }
        else if (key == ConsoleKey.D9)
        {
            return;
        }
    }

    public async Task BuyProductAsync(int productId)
    {
        using var db = new WebStoreContext();

        if (Session.CurrentCustomer == null)
        {
            ShowError("Du måste vara inloggad för att handla");
            return;
        }

        var quantity = await ProductListView.BuyProductViewAsync(productId);
        if (quantity <= 0)
            return;

        var product = await db.Products.FirstAsync(p => p.Id == productId);

        var customerId = Session.CurrentCustomer.Id;

        var cart = await db.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId };
            db.Carts.Add(cart);
            await db.SaveChangesAsync();
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.TotalPrice = existingItem.Quantity * product.Price;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = quantity * product.Price
            });
        }

        product.StockQuantity -= quantity;
        await db.SaveChangesAsync();
    }
}