using WebStore.GUI;

namespace WebStore.Controllers;

public class CategoryController : ControllerBase
{
    protected override void DrawView()
    {
        HeaderView.Show();
        // Async-metod för att hämta och visa kategorier
        // Eftersom DrawView() är synkron måste vi "brygga" async till sync
        // Detta görs med GetAwaiter().GetResult()
        CategoryView.ShowAsync().GetAwaiter().GetResult();
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
                int selectedCategoryId = 1;
                new ProductController(selectedCategoryId).Run();
                return true;
            case ConsoleKey.D2:
                int selectedCategoryId2 = 2;
                new ProductController(selectedCategoryId2).Run();
                return true;
            case ConsoleKey.D3:
                int selectedCategoryId3 = 3;
                new ProductController(selectedCategoryId3).Run();
                return true;
            case ConsoleKey.D4:
                int selectedCategoryId4 = 4;
                new ProductController(selectedCategoryId4).Run();
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }
}