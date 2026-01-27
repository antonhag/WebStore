using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class CheckoutController : ControllerBase
{
    protected override void DrawView()
    {
        CheckoutView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                ConfirmCheckout(1);
                return true;
            case ConsoleKey.D2:
                ConfirmCheckout(2);
                return true;
            case ConsoleKey.D3:
                ConfirmCheckout(3);
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private void ConfirmCheckout(int paymentMethodId)
    {
        CheckCreditCard(paymentMethodId);
        
        CheckoutView.ConfirmCheckoutView();
        
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.J:
                Checkout(paymentMethodId);
                break;
            case ConsoleKey.N:
                break;
            default:
                ShowError("Ogiltigt val!");
                break;
        }
    }

    private void Checkout(int paymentMethodId)
    {
        using var db = new WebStoreContext();
        
        CheckCreditCard(paymentMethodId);
        
        var cart = db.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefault(c => c.CustomerId == Session.CurrentCustomer.Id);

        if (Session.IsShippingAddressChanged)
        {
            var shippingAddress = Session.TemporaryShippingAddress;
        }

        if (Session.CurrentCustomer?.CityId == null)
        {
            ShowError("Ingen stad är angiven för kunden!");
            return;
        }
        
        // Räknar ut totala priset med moms plus frakt
        decimal subtotal = cart.Items.Sum(i => i.TotalPrice);
        decimal shipping = Session.SelectedDeliveryOption?.Cost ?? 0; // Fraktkostnad, om ingen metod är vald sätts kostnaden till 0 (vilket inte kan ske)
        decimal taxRate = 0.25m; // 25% moms
        decimal taxAmount = subtotal * taxRate;
        decimal total = subtotal + taxAmount + shipping;
        
        var order = new Order
        {
            CustomerId = Session.CurrentCustomer.Id,
            PaymentMethodId = paymentMethodId,
            DeliveryOptionId = Session.SelectedDeliveryOption?.Id ?? 0, // Säkerställer att databasen får ett giltigt int även om ingen leverans är vald
            DeliveryStreet = Session.IsShippingAddressChanged
                ? Session.TemporaryShippingAddress
                : Session.CurrentCustomer.Street,
            DeliveryCityId = Session.CurrentCustomer.CityId.Value,
            OrderDate = DateTime.Now,
            TotalAmount = total,
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        // Minska lagret för varje produkt som kunden köpt
        foreach (var item in cart.Items)
        {
            item.Product.StockQuantity -= item.Quantity;
        }
        
        db.Orders.Add(order);
        db.Carts.Remove(cart);
        db.SaveChanges();
        
        CheckoutView.CheckoutCompletedView(order);
        Console.ReadKey();
        
        new HomeController().Run();
    }

    public static void CheckCreditCard(int paymentMethodId)
    {
        using var db = new WebStoreContext();
        
        if (paymentMethodId == 1 && Session.CurrentCustomer.CreditCards.Count == 0)
        {
            var newCard = CheckoutView.CreditCardView();
            
            db.CreditCards.Add(newCard);
            db.SaveChanges();
            
            Session.CurrentCustomer.CreditCards.Add(newCard);

        }
    }
}