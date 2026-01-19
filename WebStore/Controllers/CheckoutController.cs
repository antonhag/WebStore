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
            TotalAmount = cart.Items.Sum(i => i.TotalPrice),
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
        
        db.Orders.Add(order);
        db.Carts.Remove(cart);
        db.SaveChanges();
        
        CheckoutView.CheckoutCompletedView(order);
        Console.ReadKey();
        
        new HomeController().Run();
    }
}