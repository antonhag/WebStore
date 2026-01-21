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

    protected override void DrawView()
    {
        _dealsList = HeaderView.ShowWithDeals();
        ProductListView.Show(_categoryId);
    }

    protected override bool HandleInput()
    {
        using var db = new WebStoreContext();
        var products = db.Products.Where(p => p.CategoryId == _categoryId).OrderBy(p => p.Id).ToList();

        var key = Console.ReadKey(true).KeyChar;
        char upperKey = char.ToUpper(key);
        
        if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
        {
            DealsHelper.HandleDealInput(_dealsList, upperKey);
            return true;
        }

        if (char.IsDigit(upperKey))
        {
            int index = upperKey - '0';

            if (index >= 1 && index <= products.Count)
            {
                int productId = products[index - 1].Id;
                
                ProductListView.ShowDetails(productId);
                var key2 = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (key2 == 'B')
                {
                    BuyProduct(productId);
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

    private void BuyProduct(int productId)
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
        
        db.SaveChanges();
    }
}
