using WebStore.Data;
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
                ManageSelectedProducts();
                return true;
            case ConsoleKey.D5:
                return true;
            case ConsoleKey.D9:
                return false;
            default:
                ShowError("Ogiltigt val!");
                return true;
        }
    }

    private void ManageSelectedProducts()
    {
        while (true)
        {
            AdminView.ShowAllProducts();
            
            Console.SetCursorPosition(21, 26);
            var input = Console.ReadLine();
        
            if (input?.ToUpper() == "B") // För att gå tillbaka till menyn
            {
                break;
            }
            
            if (!int.TryParse(input, out int productId))
            {
                ShowError("Ogiltigt ID! Ange endast siffror!");
                return;
            }
            
            using var db = new WebStoreContext();
            var product = db.Products.FirstOrDefault(p => p.Id == productId);

            int selectedCount = db.Products.Count(p => p.SelectedProduct); // Kollar hur många produkter som är selected
            if (product.SelectedProduct == false && selectedCount >= 3) // Ifall användaren försöker markera fler än 3 produkter, gör detta
            {
                ShowError("Endast 3 produkter kan markeras som utvalda");
                continue;
            }

            if (product == null)
            {
                ShowError("Produkten hittades inte!");
                continue;
            }
        
            product.SelectedProduct = !product.SelectedProduct; // Sätter den till motsatsen av vad den är nu
            db.SaveChanges();
        }
    }
}