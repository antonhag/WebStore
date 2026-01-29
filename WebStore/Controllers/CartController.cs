using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class CartController : ControllerBase
{
    protected override async Task DrawViewAsync()
    {
        HeaderView.ShowShopName();

        var cart = await GetCartAsync();

        if (cart == null || cart.Items.Count == 0)
        {
            var emptyWindow = new Window(
                "Varukorg",
                2,
                10,
                new List<string>
                {
                    "Din varukorg är tom",
                    "Tryck valfri knapp för att gå tillbaka"
                });

            emptyWindow.Draw();
            Console.ReadKey(true);
            var customerController = new CustomerController();
            await customerController.RunAsync();
        }
                
        CartView.Show(cart);
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.D9)
            {
                return false;
            }

            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D8)
            {
                await ChangeQuantityAsync(key);
                return true;
            }

            switch (key)
            {
                case ConsoleKey.C:
                    var shippingController = new ShippingController();
                    await shippingController.RunAsync();
                    return true;
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

    private static async Task<Cart> GetCartAsync()
    {
        using var db = new WebStoreContext();
        
        return await db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == Session.CurrentCustomer.Id);
    }
    
    private static async Task ChangeQuantityAsync(ConsoleKey key)
    {
        using var db = new WebStoreContext();

        var cartItems = await db.CartItems.Include(ci => ci.Product)
            .Where(ci => ci.Cart.CustomerId == Session.CurrentCustomer.Id).ToListAsync();

        int index = key - ConsoleKey.D0;

        if (index < 1 || index > cartItems.Count)
        {
            return;
        }

        var item = cartItems[index - 1];
        int newQuantity = CartView.ChangeQuantityView(item.Product);

        if (newQuantity == 0)
        {
            db.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = newQuantity;
            item.TotalPrice = newQuantity * item.Product.Price;
        }

        await db.SaveChangesAsync();
    }
}