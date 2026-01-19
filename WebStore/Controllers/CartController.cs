using WebStore.GUI;

namespace WebStore.Controllers;

public class CartController : ControllerBase
{
    protected override void DrawView()
    {
        HeaderView.Show();
        CartView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.C:
                new ShippingController().Run();
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }
}