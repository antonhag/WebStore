using WebStore.GUI;

namespace WebStore.Controllers;

public class HomeController : ControllerBase
{
    protected override void DrawView()
    {
        HomeView.Show();
        CustomerView.Show();
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
                //ToDO
                return true;
            case ConsoleKey.D2:
                new CategoryController().Run();
                return true;
            case ConsoleKey.D3:
                //ToDO
                return true;
            case ConsoleKey.D4:
                //ToDO
                return true;
            case ConsoleKey.X:
                new AdminController().Run();
                return true;
            default:
                ShowError("Ogiltigt val!");
                return true;
            
            
        }
    }
}