using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Controllers;

public class CategoryController : ControllerBase
{
    private List<(char key, Product product)> _dealsList;
    
    protected override void DrawView()
    {
        _dealsList = HeaderView.ShowWithDeals();
        // Async-metod för att hämta och visa kategorier
        // Eftersom DrawView() är synkron måste vi "brygga" async till sync
        // Detta görs med GetAwaiter().GetResult()
        CategoryView.ShowAsync().GetAwaiter().GetResult();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).KeyChar;
        char upperKey = char.ToUpper(key);
        
        if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
        {
            DealsHelper.HandleDealInput(_dealsList, upperKey);
            return true;
        }
        
        switch (upperKey)
        {
            case '1':
                new ProductController(1).Run();
                return true;
            case '2':
                new ProductController(2).Run();
                return true;
            case '3':
                new ProductController(3).Run();
                return true;
            case '4':
                new ProductController(4).Run();
                return true;
            case 'S':
                SearchProduct();
                return true;
            case '9':
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private async Task SearchProduct()
    {
        var searchedText = CategoryView.SearchView();
        
        using var db = new WebStoreContext();
        // Söker i databasen efter produkter vars namn innehåller det som användaren skrev (fritextsökning).
        var products = await db.Products.Where(p => EF.Functions.Like(p.Name, $"%{searchedText}%")).OrderBy(p => p.Name).ToListAsync();

        await MongoLogger.LogProductSearchAsync(searchedText, Session.CurrentCustomer);
        
        if (products.Count < 1)
        {
            CategoryView.SearchError("Varan hittades ej");
            Console.ReadKey(true);
            return;
        }
        
        CategoryView.ShowSearchResults(products);
        HandleSearchInput(products);
    }

    private void HandleSearchInput(List<Product> searchedProducts)
    {
        var key = Console.ReadKey(true).Key;
        int selectedIndex = (int)key - (int)ConsoleKey.D0; // D1 numeriska värde = 49. D1(49) - D0(48) = 1. D2(50) - D0(48) = 2 osv...

        if (selectedIndex > 0 && selectedIndex <= searchedProducts.Count)
        {
            int productId = searchedProducts[selectedIndex - 1].Id;
            
            ProductListView.ShowDetails(productId);
            var key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.B)
            {
                BuyProduct(productId);
            }
        }
        else if (key == ConsoleKey.D9)
        {
            return;
        }
    }
    
    public void BuyProduct(int productId)
    {
        using var db = new WebStoreContext();

        if (Session.CurrentCustomer == null)
        {
            ShowError("Du måste vara inloggad för att handla");
            return;
        }

        var quantity = ProductListView.BuyProductView(productId);
        if (quantity <= 0)
            return;

        var product = db.Products.First(p => p.Id == productId);
        
        var customerId = Session.CurrentCustomer.Id;

        var cart = db.Carts
            .Include(c => c.Items)
            .FirstOrDefault(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId };
            db.Carts.Add(cart);
            db.SaveChanges();
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
        db.SaveChanges();
    }
}