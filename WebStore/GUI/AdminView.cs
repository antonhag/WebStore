using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class AdminView
{
    public enum AdminMenu
    {
        Adminstrera_produkter = 1,
        Adminstrera_kategorier,
        Adminstrera_kunder,
        Ändra_utvalda_produkter,
        Se_statistik,
        Återgå_till_startsida = 9
    }

    public static void Show()
    {
        var adminOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(AdminMenu)))
        {
            adminOptions.Add($"{i}. {Enum.GetName(typeof(AdminMenu), i).Replace("_", " ")}");
        }

        var adminWindow = new Window("Admin", 2, 2, adminOptions);
        
        adminWindow.Draw();
    }

    public static void ShowAllProducts()
    {
        Console.Clear();
        var productList  = new List<string>();

        using (var db = new WebStoreContext())
        {
            var products = db.Products.ToList();
            
            productList.Add(
                "ID".PadRight(10) +
                "Produkt".PadRight(25) +
                "Pris".PadRight(15) +
                "Utvald"
            );
            
            productList.Add(new string('-', 60));
            
            foreach (var product in products)
            {
                productList.Add(
                    product.Id.ToString().PadRight(10) +
                    product.Name.PadRight(25) +
                    ($"{product.Price} kr").PadRight(15) +
                    (product.SelectedProduct ? "   X" : "")
                );
            }
            productList.Add(new string('-', 60));
            productList.Add("Ange Produkt-ID: ");
            productList.Add("X = Utvald produkt");
            productList.Add("B. Gå tillbaka till menyn");
            
            var window =  new Window("Alla produkter | Ange Produkt-ID för att ändra status", 2, 2, productList);
            window.Draw();
        }
    }
}