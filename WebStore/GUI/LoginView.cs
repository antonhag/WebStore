namespace WebStore.GUI;

public class LoginView
{
    public enum LoginMenu
    {
        Logga_in_som_kund = 1,
        Logga_in_som_admin,
        Avsluta = 9
    }

    public static void Show()
    {
        var header = new Window($"Hags Kläder", 2, 1, new List<string> { "Trender som håller, pris som inte slår hål!" });
        header.Draw();
        
        var loginOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(LoginMenu)))
        {
            loginOptions.Add($"{i}. {Enum.GetName(typeof(LoginMenu), i).Replace("_", " ")}");
        }

        var loginWindow = new Window("Logga in", 2, 10, loginOptions);
        
        loginWindow.Draw();
    }

    public static void ShowLogin()
    {
        var rows = new List<string>
        {
            "Ange dina uppgifter            ",
            "Email:",
            "Password:"
        };
    
        var loginWindow = new Window("Logga in", 2, 10, rows);
        loginWindow.Draw();
    }
    
    public static (string email, string password) GetCredentials()
    {
        Console.SetCursorPosition(11, 9 + 3);
        var email = Console.ReadLine();
        
        Console.SetCursorPosition(14, 9 + 4);
        var password = Console.ReadLine();
        
        return (email, password);
    }
    
}