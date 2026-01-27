using WebStore.Data;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.GUI;

public class HeaderView
{
    public static void ShowShopName()
    {
        // Ifall ingen är inloggad (första sidan) skriv ut butiksnamn endast, annars välkommen meddelande när man är inloggad
        string shopHeader = Session.CurrentCustomer != null
            ? $"Välkommen till Hags Kläder {Session.CurrentCustomer.FirstName}"
            : "Hags Kläder";

        var header = new Window(shopHeader, 2, 1, new List<string> { "Trender som håller, pris som inte slår hål!" });
        header.Draw();
    }
    
    public static List<(char key, Product product)> ShowWithDeals()
    {
        ShowShopName();
        return DealsHelper.ShowDeals();
    }
}

