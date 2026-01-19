using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class ShippingController : ControllerBase
{
    protected override void DrawView()
    {
        Console.Clear();
        ShippingView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.J:
                var newAddress = ShippingView.ChangeShippingAddressView();
                if (!string.IsNullOrWhiteSpace(newAddress))
                {
                    Session.TemporaryShippingAddress = newAddress;
                    Session.IsShippingAddressChanged = true;
                }
                return true;
            case ConsoleKey.N:
                HandleDeliveryInput();
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val! FRAKT");
                return true;
        }
    }
    
    private void HandleDeliveryInput()
    {
        ShippingView.ShowDeliveryOptions();
        
        var key = Console.ReadKey(true).Key;

        int deliveryOptionId;

        switch (key)
        {
            case ConsoleKey.D1:
                deliveryOptionId = 1;
                break;
            case ConsoleKey.D2:
                deliveryOptionId = 2;
                break;
            case ConsoleKey.D3:
                deliveryOptionId = 3;
                break;
            case ConsoleKey.D9:
                return;
            default:
                ShowError("Ogiltigt val!");
                return;
        }

        using var db = new WebStoreContext();

        var deliveryOption = db.DeliveryOptions.FirstOrDefault(d => d.Id == deliveryOptionId);

        if (deliveryOption == null)
        {
            ShowError("Fraktalternativ hittades ej!");
            return;
        }
        
        Session.SelectedDeliveryOption = deliveryOption;

        new CheckoutController().Run();
    }
}