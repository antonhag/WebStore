using WebStore.Controllers;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.GUI;

public class LoginView
{
    private enum LoginMenu
    {
        Logga_in_som_kund = 1,
        Ny_kund,
        Logga_in_som_admin,
        Avsluta = 9
    }

    public static void Show()
    {
        HeaderView.ShowShopName();

        var loginOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(LoginMenu)))
        {
            loginOptions.Add($"{i}. {Enum.GetName(typeof(LoginMenu), i).Replace("_", " ")}");
        }

        var loginWindow = new Window("Logga in", 2, 10, loginOptions);

        loginWindow.Draw();
    }

    public static void ShowLoginCustomer()
    {
        Console.Clear();
        HeaderView.ShowShopName();

        var rows = new List<string>
        {
            "Ange dina uppgifter            ",
            "Email:",
            "Lösenord:"
        };

        var loginWindow = new Window("Logga in som kund", 2, 10, rows);
        loginWindow.Draw();
    }

    public static (string emailOrUserName, string password) GetCredentials()
    {
        Console.SetCursorPosition(11, 9 + 3);
        var emailOrUserName = Console.ReadLine();

        Console.SetCursorPosition(14, 9 + 4);
        string password = PasswordHelper.ReadPassword();

        return (emailOrUserName, password);
    }

    public static void ShowLoginAdmin()
    {
        Console.Clear();
        HeaderView.ShowShopName();

        var rows = new List<string>
        {
            "Ange dina uppgifter            ",
            "A-namn:",
            "Lösenord:"
        };

        var loginWindow = new Window("Logga in som admin", 2, 10, rows);
        loginWindow.Draw();
    }

    public static (string firstName, string lastName, string? phoneNumber, string email, string password, DateTime
        birthDate, string street, string zipCode, int cityId) NewCustomerView(List<City> cities)
    {
        Console.WriteLine("Skapa ny kund\n");

        Console.Write("Förnamn: ");
        string firstName = Console.ReadLine() ?? ""; // säkerställer att det alltid blir en sträng och aldrig null

        Console.Write("Efternamn: ");
        string lastName = Console.ReadLine() ?? "";

        // Telefonnummer (valfritt)
        Console.Write("Telefonnummer (valfritt, tryck Enter för att hoppa över): ");
        string? phoneNumber = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(phoneNumber))
            phoneNumber = null;

        string email;
        while (true)
        {
            Console.Write("E-post: ");
            email = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(email))
                break;
            Console.WriteLine("E-post får inte vara tom!");
        }

        string password;
        while (true)
        {
            Console.Write("Lösenord: ");
            password = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(password))
                break;
            Console.WriteLine("Lösenord får inte vara tomt!");
        }

        DateTime birthDate;
        while (true)
        {
            Console.Write("Födelsedatum (yyyy-mm-dd): ");
            string birthInput = Console.ReadLine() ?? "";
            if (DateTime.TryParse(birthInput, out birthDate))
                break;
            Console.WriteLine("Ogiltigt datumformat!");
        }
        
        string street;
        while (true)
        {
            Console.Write("Gatuadress: ");
            street = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(street))
                break;
            Console.WriteLine("Gatuadress får inte vara tomt!");
        }

        string zipCode;
        while (true)
        {
            Console.Write("Postnummer: ");
            zipCode = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(zipCode))
                break;
            Console.WriteLine("Postnummer inte vara tomt!");
        }
        
        Console.WriteLine("Välj stad från listan nedan\n");

        foreach (var city in cities)
        {
            Console.WriteLine($"{city.Id}: {city.Name}");
        }

        int cityId;
        while (true)
        {
            Console.Write("\nAnge stads-ID: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out var chosenId) && cities.Any(c => c.Id == chosenId))
            {
                cityId = chosenId;
                break;
            }

            Console.WriteLine("Ogiltigt ID, försök igen");
        }

        return (firstName, lastName, phoneNumber, email, password, birthDate, street, zipCode, cityId);
    }
}