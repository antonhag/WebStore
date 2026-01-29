using WebStore.GUI;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Controllers;

public class CustomerController : ControllerBase
{
    private List<(char key, Product product)> _dealsList;
    
    protected override async Task DrawViewAsync()
    {
        _dealsList = await HeaderView.ShowWithDealsAsync(); // Ritar upp deals och returnerar listan av tangent med produkt-kopplingar
        CustomerView.Show();
    }
    
    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            var key = Console.ReadKey(true).KeyChar;
            char upperKey = char.ToUpper(key);

            if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
            {
                await DealsHelper.HandleDealInput(_dealsList, upperKey); // Hanterar köp av vald deal baserat på tangent
                return true;
            }

            switch (key)
            {
                case '1':
                    var categoryController = new CategoryController();
                    await categoryController.RunAsync();
                    return true;
                case '2':
                    var cartController = new CartController();
                    await cartController.RunAsync();
                    return true;
                case '9':
                    Session.Logout();
                    var loginController = new LoginController();
                    await loginController.RunAsync();
                    return false;
                default:
                    ShowError("Ogiltigt val!");
                    return true;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Ett fel inträffade: {ex.Message}");
            return true;
        }
    }
    
}