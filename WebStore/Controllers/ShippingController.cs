using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class ShippingController : ControllerBase
{
    protected override async Task DrawViewAsync()
    {
        Console.Clear();
        ShippingView.Show();
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
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
                    await HandleDeliveryInputAsync();
                    return true;
                case ConsoleKey.D9:
                    return false;
                default:
                    ShowError("Ogiltigt val! FRAKT");
                    return true;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Ett fel inträffade: {ex.Message}");
            return true;
        }
   
    }
    
    private async Task HandleDeliveryInputAsync()
    {
        await ShippingView.ShowDeliveryOptions();
        
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

        var deliveryOption = await db.DeliveryOptions.FirstOrDefaultAsync(d => d.Id == deliveryOptionId);

        if (deliveryOption == null)
        {
            ShowError("Fraktalternativ hittades ej!");
            return;
        }
        
        Session.SelectedDeliveryOption = deliveryOption;

        await new CheckoutController().RunAsync();
    }
}