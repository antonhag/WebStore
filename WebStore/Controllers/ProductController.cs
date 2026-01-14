using WebStore.Data;
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
        using var db = new WebStoreContext();
        var products = db.Products.Where(p => p.CategoryId == _categoryId).OrderBy(p => p.Id).ToList();

        var key = Console.ReadKey(true).Key;
        int selectedIndex = (int)key - (int)ConsoleKey.D0; // D1 numeriska värde = 49. D1(49) - D0(48) = 1. D2(50) - D0(48) = 2 osv...

        if (selectedIndex > 0 && selectedIndex <= products.Count)
        {
            int productId = products[selectedIndex - 1].Id;
            ProductListView.ShowDetails(productId);
            var key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.B)
            {
                
            }
            return true;
        }

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
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private void BuyProduct(int productId)
    {
        
    }
}
