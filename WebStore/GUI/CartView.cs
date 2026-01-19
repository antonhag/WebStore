using Microsoft.EntityFrameworkCore;
using WebStore.Controllers;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CartView
{
    public static void Show()
    {
        if (Session.CurrentCustomer == null)
        {
            Console.WriteLine("Ingen kund är inloggad");
            Console.ReadKey();
            return;
        }
        
        using var db = new WebStoreContext();
        
        var cart = db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault(c => c.CustomerId == Session.CurrentCustomer.Id);

        if (cart == null || cart.Items.Count == 0)
        {
            var emptyWindow = new Window("Varukorg", 2, 10,
                new List<string> { "Din varukorg är tom", "Tryck valfri knapp för att gå tillbaka" });
            emptyWindow.Draw();
            Console.ReadKey(true);
            
            new HomeController().Run();
        }

        var rows = new List<string>();
        decimal total = 0;

        int index = 1;
        foreach (var item in cart.Items)
        {
            rows.Add($"{index}. {item.Product.Name}");
            rows.Add($"   Antal: {item.Quantity}  Pris: {item.TotalPrice} kr");
            total += item.TotalPrice;
            index++;
        }

        rows.Add("------------------------------------------");
        rows.Add($"Totalt: {total}");
        rows.Add("C. Gå till kassan");
        rows.Add("9. Gå tillbaka till menyn");

        var cartWindow = new Window("Varukorg | Ange Produkt-Id för att ändra antal", 2, 10, rows);
        cartWindow.Draw();
    }
}