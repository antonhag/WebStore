using WebStore.Data;

namespace WebStore.GUI;

public class HeaderView
{
    public static void Show()
    {
        var header = new Window("Hags Kläder", 2, 1, new List<string> { "# Fina butiken #, Allt inom kläder" });
        header.Draw();

        var dealList = new List<string>();
        
        using (var db = new WebStoreContext())
        {
            var deals = db.Products.Where(p => p.SelectedProduct == true).ToList();

            int left = 2;
            int top = 4;
            int dealNumber = 1;
            char buttonKey = 'A';
            
            foreach (var deal in deals)
            {
                var dealWindow = new Window(
                    "Erbjudande " + dealNumber,  
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
                left += 26;
                dealNumber++;
                buttonKey++;
            }
        }

        var admin = new Window("Admin", 2, 25, new List<string> { "Tryck X för att logga in som admin" });
        admin.Draw();
    }
}

