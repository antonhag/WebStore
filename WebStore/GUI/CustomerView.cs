namespace WebStore.GUI;

public class CustomerView
{
    private enum CustomerMenu
    {
        Shoppen = 1,
        Varukorgen,
        Logga_ut = 9
    }

    public static void Show()
    {
        var menuOptions = new List<string>();
        
        foreach (int i in Enum.GetValues(typeof(CustomerMenu)))
        {
            menuOptions.Add($"{i}. {Enum.GetName(typeof(CustomerMenu), i).Replace("_", " ")}");
        }
        
        var menuWindow = new Window("Kundmeny", 2, 10, menuOptions);
        
        menuWindow.Draw();
    }
}