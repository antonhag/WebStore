using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;

namespace WebStore.GUI;

public class ShippingView
{
    public bool IsAddressChanged { get; set; }
    
    public static void Show()
    {
        Console.Clear();
        HeaderView.ShowShopName();
        
        var customer = Session.CurrentCustomer;

        // Ternary operator som kollar ifall adressen är ändrad, (true = skriv ut den nya), (false = skriv ut den befintliga adressen)
        string addressToShow = Session.IsShippingAddressChanged ? Session.TemporaryShippingAddress : customer.Street;
        
        var customerInfo = new List<string>
        {
            $"Namn: {customer.FirstName} {customer.LastName}",
            $"Stad: {customer.City.Name}",
            $"Adress: {addressToShow}",
            $"Postnummer: {customer.ZipCode}",
            "Vill du ändra adress? (j/n)",
            "Tryck 9 för att gå tillbaka till menyn"
        };

        var infoWindow = new Window("Leveransadress", 2, 10, customerInfo);
        infoWindow.Draw();
    }

    public static string ChangeShippingAddressView()
    {
        Console.Clear();
        HeaderView.ShowShopName();
    
        var addressWindow = new Window("Skriv in din nya leveransadress", 2, 10, new List<string> { "Adress: " });
        addressWindow.Draw();
        Console.SetCursorPosition(12, 11);
        string newAddress = Console.ReadLine();
    
        var confirmationWindow = new Window("Lyckades!", 2, 10,
            new List<string>
                { $"Adressen uppdaterad till: {newAddress}", "Tryck på valfri tangent för att fortsätta..." });
        Console.Clear();
        confirmationWindow.Draw();
        Console.ReadKey(true);
        
        return newAddress;
    }
    
    public static async Task ShowDeliveryOptions()
    {
        Console.Clear();
        HeaderView.ShowShopName();
        
        var deliveryOptionList = new List<string>();
        
        using (var db = new WebStoreContext())
        {
            var deliveryOption = await db.DeliveryOptions.ToListAsync();
            
            foreach (var option in deliveryOption)
            {
                deliveryOptionList.Add($"{option.Id}. {option.Name} - {option.Description} - {option.EstimatedTime} - {option.Cost} kr");
            }
            deliveryOptionList.Add("9. för att gå tillbaka till menyn");
        }
        
        var deliveryWindow = new Window("Fraktalternativ | Ange frakt-Id för det alternativet du vill välja", 2, 10, deliveryOptionList);
        deliveryWindow.Draw();
    }
}