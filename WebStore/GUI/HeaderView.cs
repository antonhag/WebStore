using WebStore.Data;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.GUI;

public class HeaderView
{
    public static void ShowShopName()
    {
        var header = new Window($"Hags Kläder", 2, 1, new List<string> { "Trender som håller, pris som inte slår hål!" });
        header.Draw();
    }
    
    public static List<(char key, Product product)> ShowWithDeals()
    {
        ShowShopName();
        return DealsHelper.ShowDeals();
    }
}

