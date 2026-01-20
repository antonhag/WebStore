using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class HeaderView
{
    public static void Show()
    {
        ShowShopName();
        ShowSelectedProducts();
    }

    public static void ShowShopName()
    {
        var header = new Window($"Hags Kläder", 2, 1, new List<string> { "Trender som håller, pris som inte slår hål!" });
        header.Draw();
    }

    public static List<(char key, Product product)> ShowSelectedProducts()
    {
        List<(char key, Product product)> dealsList = new List<(char, Product)>();
        
        using (var db = new WebStoreContext())
        {
            var deals = db.Products.Where(p => p.SelectedProduct == true).ToList();

            int left = 2;
            int top = 4;
            char buttonKey = 'A';
            
            foreach (var deal in deals)
            {
                var dealWindow = new Window(
                    "Erbjudande " + (buttonKey - 'A' +1),  
                    left,
                    top,
                    new List<string>
                    {
                        deal.Name,
                        deal.Description ?? "",       
                        $"Pris: {deal.Price} kr",
                        $"Tryck {buttonKey} för att köpa"
                    });
                
                dealWindow.Draw();
                
                dealsList.Add((buttonKey, deal));
                
                left += 26;
                buttonKey++;
            }
        }
        return dealsList;
    }
}

