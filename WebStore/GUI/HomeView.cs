namespace WebStore.GUI;

public class HomeView
{
    public void Show()
    {
        var header = new Window("Hags Kläder", 2, 1, new List<string> { "# Fina butiken #, Allt inom kläder" });
        
        var deal1 = new Window("Erbjudande 1", 2, 4,
            new List<string> { "Tröja", "Fin tröja i bomull", "Pris: 149 kr", "Tryck A för att köpa" });
        
        var deal2 = new Window("Erbjudande 2", 28, 4,
            new List<string> { "Byxor", "Skön passform", "Pris: 499 kr", "Tryck B för att köpa" });
        
        var deal3 = new Window("Erbjudande 3", 56, 4,
            new List<string> { "Skor", "Bra dämpning", "Pris: 799 kr", "Tryck C för att köpa" });
        
        header.Draw();
        deal1.Draw();
        deal2.Draw();
        deal3.Draw();
        CustomerView.Show();
        //CategoryView.Show();
        //AdminView.Show();
    }
}

