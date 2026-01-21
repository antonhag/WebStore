using WebStore.GUI;
using WebStore.Helpers;
using WebStore.Models;

namespace WebStore.Controllers;

public class HomeController : ControllerBase
{
    private List<(char key, Product product)> _dealsList;
    
    protected override void DrawView()
    {
        _dealsList = HeaderView.ShowWithDeals(); // Ritar upp deals och returnerar listan av tangent med produkt-kopplingar
        CustomerView.Show();
    }
    
    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).KeyChar;
        char upperKey = char.ToUpper(key);
        
        if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
        {
            DealsHelper.HandleDealInput(_dealsList, upperKey); // Hanterar köp av vald deal baserat på tangent
            return true;
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