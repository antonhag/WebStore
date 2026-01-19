using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CheckoutView
{
    public static void Show()
    {
        Console.Clear();
        HeaderView.ShowShopName();

        using var db = new WebStoreContext();
        
        var checkoutCart = db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault(c => c.CustomerId == Session.CurrentCustomer.Id);
        
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
        rows.Add($"Totalt: {total} kr");
        
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
            $"Totalt belopp: {order.TotalAmount} kr",
            $"Leveransadress: {order.DeliveryStreet}",
            "Du har fått ett mejl med orderbekräftelse.",
            "Tryck valfri knapp för att fortsätta..."
        };

        var completedWindow = new Window("Ordern har skapats", 2, 10, confirmationRows);
        completedWindow.Draw();
        
    }
}