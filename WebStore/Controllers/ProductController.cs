using WebStore.GUI;

namespace WebStore.Controllers;

public class ProductController : ControllerBase
{
    private readonly int _categoryId; // Readonly = att den endast kan sättas i konstruktorn och ändras ej
    
    public ProductController(int categoryId)
    {
        _categoryId = categoryId;
    }
    
    protected override void DrawView()
    {
        HeaderView.Show();
        ProductListView.Show(_categoryId);
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