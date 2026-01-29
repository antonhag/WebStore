namespace WebStore.Helpers;

public class PasswordHelper
{
    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(true);

            // Enter avslutar
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            // Backspace
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b"); // ta bort *
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        return password;
    }

}