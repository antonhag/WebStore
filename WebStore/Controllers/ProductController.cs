using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Controllers;

public class ProductController : ControllerBase
{
    private readonly int _categoryId; // Readonly = att den endast kan sättas i konstruktorn och ändras ej
    private List<(char key, Product product)> _dealsList;
    
    public ProductController(int categoryId)
    {
        _categoryId = categoryId;
    }

    protected override async Task DrawViewAsync()
    {
        _dealsList = await HeaderView.ShowWithDealsAsync();
        await ProductListView.ShowAsync(_categoryId);
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            using var db = new WebStoreContext();
            var products = await db.Products.Where(p => p.CategoryId == _categoryId).OrderBy(p => p.Id).ToListAsync();
            
            var key = Console.ReadKey(true).KeyChar;
            char upperKey = char.ToUpper(key);
        
            if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
            {
                await DealsHelper.HandleDealInput(_dealsList, upperKey);
                return true;
            }

            if (char.IsDigit(upperKey))
            {
                int index = upperKey - '0';

                if (index >= 1 && index <= products.Count)
                {
                    int productId = products[index - 1].Id;
                
                    await ProductListView.ShowDetailsAsync(productId);
                    var key2 = char.ToUpper(Console.ReadKey(true).KeyChar);
                    if (key2 == 'B')
                    {
                        await BuyProductAsync(productId);
                    }
                    return true;
                }
            }

            switch (upperKey)
            {
                case '9':
                    return false;
                default:
                    ShowError("Ogiltigt val!");
                    return true;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Ett fel inträffade:  {ex.Message}");
            return true;
        }
    }

    private async Task BuyProductAsync(int productId)
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
        
        await db.SaveChangesAsync();
    }
}
