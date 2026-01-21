using WebStore.Controllers;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Helpers;

public static class DealsHelper
{
    public static List<(char key, Product product)>
        ShowDeals() // Ritar upp deals och returnerar en lista av produkter med koppling till A, B, C
    {
        List<(char key, Product product)> dealsList = new List<(char, Product)>();

        using (var db = new WebStoreContext())
        {
            var deals = db.Products.Where(p => p.SelectedProduct == true).ToList();

            int left = 2;
            int top = 4;
            char buttonKey = 'A';

            foreach (var deal in deals)
            {
                var dealWindow = new Window(
                    "Erbjudande " + (buttonKey - 'A' + 1), // Räknar om bokstav till nummer
                    left,
                    top,
                    new List<string>
                    {
                        deal.Name,
                        deal.Description ?? "",
                        $"Pris: {deal.Price} kr",
                        $"Tryck {buttonKey} för att köpa"
                    });

                dealWindow.Draw();

                dealsList.Add((buttonKey, deal)); // Här sker kopplingen mellan en produkt och tex 'A'

                left += 26;
                buttonKey++;
            }
        }

        return dealsList;
    }

    public static void HandleDealInput(List<(char key, Product product)> dealsList,
        char keyPressed) // Metod för att hantera köp av den dealen som är vald
    {
        var upperKey = char.ToUpper(keyPressed);

       var selectedDeal = dealsList.FirstOrDefault(p => p.key == upperKey);

       if (selectedDeal.product == null)
       {
           return; // Extra säkerhet, bör ej inträffa
       }
       
       var categoryController = new CategoryController();
       categoryController.BuyProduct(selectedDeal.product.Id);
    }
}