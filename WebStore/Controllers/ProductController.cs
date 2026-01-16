using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class ProductController : ControllerBase
{
    private readonly int _categoryId; // Readonly = att den endast kan sättas i konstruktorn och ändras ej

    public ProductController(int categoryId)
    {
        _categoryId = categoryId;
    }

    protected override void DrawView()
    {
        HeaderView.Show();
        ProductListView.Show(_categoryId);
    }

    protected override bool HandleInput()
    {
        using var db = new WebStoreContext();
        var products = db.Products.Where(p => p.CategoryId == _categoryId).OrderBy(p => p.Id).ToList();

        var key = Console.ReadKey(true).Key;
        int selectedIndex = (int)key - (int)ConsoleKey.D0; // D1 numeriska värde = 49. D1(49) - D0(48) = 1. D2(50) - D0(48) = 2 osv...

        if (selectedIndex > 0 && selectedIndex <= products.Count)
        {
            int productId = products[selectedIndex - 1].Id;
            ProductListView.ShowDetails(productId);
            var key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.B)
            {
                BuyProduct(productId);
            }
            return true;
        }

        switch (key)
        {
            case ConsoleKey.A:
                //ToDO
                return true;
            case ConsoleKey.B:
                //ToDO
                return true;
            case ConsoleKey.C:
                //ToDO
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private void BuyProduct(int  productId)
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

        if (product.StockQuantity < quantity)
        {
            ProductListView.BuyProductView(productId);
            return;
        }

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
