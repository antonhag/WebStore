using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class CartController : ControllerBase
{
    protected override void DrawView()
    {
        HeaderView.Show();
        CartView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.D9)
        {
            return false;
        }
        
        if (key >= ConsoleKey.D1 && key <= ConsoleKey.D8)
        {
            ChangeQuantity(key);
            return true;
        }

        switch (key)
        {
            case ConsoleKey.C:
                new ShippingController().Run();
                return true;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private static void ChangeQuantity(ConsoleKey key)
    {
        using var db = new WebStoreContext();
        
       var cartItems = db.CartItems.Include(ci => ci.Product).Where(ci => ci.Cart.CustomerId == Session.CurrentCustomer.Id).ToList();
       
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
       
       db.SaveChanges();
    }
}