using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class HomeController : ControllerBase
{
    private List<(char key, Product product)> selectedDeals = new List<(char, Product)>();
    
    protected override void DrawView()
    {
        HeaderView.Show();
        CustomerView.Show();

        selectedDeals = HeaderView.ShowSelectedProducts();
    }
    
    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).KeyChar;
        char upperKey = char.ToUpper(key);

        if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
        {
            var selectedDeal = selectedDeals.FirstOrDefault(d => d.key == upperKey);
            if (selectedDeal.product != null)
            {
                var categoryController = new CategoryController();
                categoryController.BuyProduct(selectedDeal.product.Id);
                return true;
            }
            else
            {
                ShowError("Denna deal finns inte längre!");
                return true;
            }
        }
        
        switch (key)
        {
            case '1':
                //ToDO
                return true;
            case '2':
                new CategoryController().Run();
                return true;
            case '3':
                new CartController().Run();
                return true;
            default:
                ShowError("Ogiltigt val!");
                return true;
            
            
        }
    }
}