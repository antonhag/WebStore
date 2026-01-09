using WebStore.GUI;

namespace WebStore.Controllers;

public class AdminController : ControllerBase
{
    protected override void DrawView()
    {
        
        AdminView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                return true;
            case ConsoleKey.D2:
                return true;
            case ConsoleKey.D3:
                return true;
            case ConsoleKey.D4:
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }
}