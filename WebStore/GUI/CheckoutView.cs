using Microsoft.EntityFrameworkCore;
using WebStore.Controllers;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CheckoutView
{
    public static async Task ShowAsync()
    {
        Console.Clear();
        HeaderView.ShowShopName();

        using var db = new WebStoreContext();
        
        var checkoutCart = await db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(c => c.CustomerId == Session.CurrentCustomer.Id);
        
        var rows = new List<string>();
        decimal subtotal = 0; // Pris utan frakt och moms
        decimal shipping = Session.SelectedDeliveryOption.Cost; // Fraktkostnad
        decimal taxRate = 0.25m; // 25 % moms

        foreach (var item in checkoutCart.Items)
        {
            rows.Add($"{item.Product.Name}");
            rows.Add($"   Antal: {item.Quantity}  Pris: {item.TotalPrice} kr");
            subtotal += item.TotalPrice;
        }
        decimal taxAmount = subtotal * taxRate; // Moms på varor
        decimal total = subtotal + taxAmount + shipping;
        
        rows.Add($"Frakt: {shipping} kr");
        rows.Add($"Moms (25%): {taxAmount:F2} kr"); // F2 för att få två decimaler
        rows.Add("------------------------------------------");
        rows.Add($"Totalt: {total:F2} kr");
        
        var productsWindow = new Window("Varukorg", 2, 4, rows);
        productsWindow.Draw();
        
        var paymentMethods = db.PaymentMethods;
        
        var rows2 = new List<string>();

        int index = 1;
        foreach (var paymentMethod in paymentMethods)
        {
            rows2.Add($"{paymentMethod.Id}. {paymentMethod.Name} - {paymentMethod.Description}");
        }
        
        var methodWindow = new Window("Välj betalningsmetod (skriv siffran)", 2, 25, rows2);
        methodWindow.Draw();
    }

    public static void ConfirmCheckoutView()
    {
        Console.Clear();
        HeaderView.ShowShopName();
        
        var confirmWindow = new Window("Genomför köp", 2, 10, new List<string> {"Tryck J för att genomföra köp", "Tryck N för att gå tillbaka"});
        confirmWindow.Draw();
    }
    
    public static void CheckoutCompletedView(Order order)
    {
        Console.Clear();
        HeaderView.ShowShopName();

        var confirmationRows = new List<string>
        {
            $"Tack för din beställning, {Session.CurrentCustomer.FirstName}!",
            $"Ordernummer: {order.Id}",
            $"Totalt belopp: {order.TotalAmount:F2} kr",
            $"Leveransadress: {order.DeliveryStreet}",
            "Du har fått ett mejl med orderbekräftelse.",
            "Tryck valfri knapp för att fortsätta..."
        };

        var completedWindow = new Window("Ordern har skapats", 2, 10, confirmationRows);
        completedWindow.Draw();
        
    }

    public static CreditCard CreditCardView()
    {
        Console.Clear();
        HeaderView.ShowShopName();

        string last4;
        DateTime expiration;
        string type;

        while (true)
        {
            Console.Clear();
            HeaderView.ShowShopName();
            var rows = new List<string>
            {   
                "Kortuppgifter                       ",
                "Ange sista 4 av kortnumret: ",
                "Ange utgångsdatum (yyyy-MM): ",
                "Ange korttyp: "
            };

            var window = new Window("Lägg till kreditkort", 2, 5, rows);
            window.Draw();
        
            Console.SetCursorPosition(32, 7); 
            last4 = Console.ReadLine()!;
            
            if (last4.Length != 4 || !last4.All(Char.IsDigit)) // Ifall last4 inte enbart är 4 siffror, gör detta
            {
                Console.Clear();
                HeaderView.ShowShopName();
                var error = new Window("Fel!", 2, 5, new List<string> { "Kortnumret måste exakt vara 4 siffror", "Tryck valfri knapp för att gå vidare..." });
                error.Draw();
                Console.ReadKey();
                continue;
            }

            Console.SetCursorPosition(33, 8);
            string expInput = Console.ReadLine()!;
            
            // + "-01"(motsvarar dag) lägger till det obligatoriska värdet för DateTime, försöker sedan konvertera strängen till ett DateTime-objekt
            if (!DateTime.TryParse(expInput + "-01", out expiration)) 
            {
                Console.Clear();
                HeaderView.ShowShopName();
                var error = new Window("Fel!", 2, 5, new List<string> { "Felaktigt datumformat! Ange yyyy-MM", "Tryck valfri knapp för att gå vidare..." });
                error.Draw();
                Console.ReadKey();
                continue;
            }

            Console.SetCursorPosition(18, 9); 
            type = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(type))
            {
                Console.Clear();
                HeaderView.ShowShopName();
                var error = new Window("Fel!", 2, 5, new List<string> { "Korttyp kan inte vara tomt!", "Tryck valfri knapp för att gå vidare..." });
                error.Draw();
                Console.ReadKey();
                continue;
            }

            break;
        }
        
        Console.Clear();
        HeaderView.ShowShopName();
        var confirmWindow = new Window("Kreditkort tillagt!", 2, 5,
            new List<string> { "Tryck valfri knapp för att gå vidare..." });
        confirmWindow.Draw();
        Console.ReadKey();
        
        return new CreditCard
        {
            CardNumberLast4 = last4,
            ExpirationDate = expiration,
            CardType = type,
            CustomerId = Session.CurrentCustomer.Id
        };

    }
}