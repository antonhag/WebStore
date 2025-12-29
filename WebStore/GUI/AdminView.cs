namespace WebStore.GUI;

public class AdminView
{
    public enum AdminMenu
    {
        Adminstrera_produkter = 1,
        Adminstrera_kategorier,
        Adminstrera_kunder,
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

        var adminWindow = new Window("Admin", 2, 4, adminOptions);
        
        adminWindow.Draw();
    }
}