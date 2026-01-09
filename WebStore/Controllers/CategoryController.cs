using WebStore.GUI;

namespace WebStore.Controllers;

public class CategoryController : ControllerBase
{
    protected override void DrawView()
    {
        HomeView.Show();
        CategoryView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.A:
                //ToDO
                return true;
            case ConsoleKey.B:
                //ToDO
                return true;
            case ConsoleKey.C:
                //ToDO
                return true;
            case ConsoleKey.D1:
                // ToDo
                return true;
            case ConsoleKey.D2:
                // ToDo
                return true;
            case ConsoleKey.D3:
                // ToDo
                return true;
            case ConsoleKey.D4:
                // ToDo
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }
}