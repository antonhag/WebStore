using WebStore.Controllers;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Helpers;

public static class DealsHelper
{
    public static List<(char key, Product product)> ShowDeals()
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
                    "Erbjudande " + (buttonKey - 'A' + 1),
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

                dealsList.Add((buttonKey, deal));

                left += 26;
                buttonKey++;
            }
        }
        return dealsList;
    }

    public static void HandleDealInput(ControllerBase controller, List<(char key, Product product)> dealsList, char keyPressed)
    {
        while (true)
        {
            var upperKey = char.ToUpper(keyPressed);

            if (upperKey == 'A' || upperKey == 'B' || upperKey == 'C')
            {
                var selectedDeal = dealsList.FirstOrDefault(d => d.key == upperKey);
                if (selectedDeal.product != null)
                {
                    var categoryController = new CategoryController();
                    categoryController.BuyProduct(selectedDeal.product.Id);
                    return;
                }
                else
                {
                    controller.ShowError("Ogiltigt val! Försök igen");
                }
            }
        }
    }
}