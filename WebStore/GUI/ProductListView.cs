using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class ProductListView
{
    public static void Show(int categoryId)
    {
        Console.Clear();
        HeaderView.Show();
        var productList = new List<string>();

        using (var db = new WebStoreContext())
        {
            var products = db.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Id).ToList();

            int index = 1;

            foreach (var product in products)
            {
                productList.Add($"{index}. {product.Name} - {product.Price} kr");
                index++;
            }

            productList.Add("9. För att gå tillbaka till menyn");
        }

        var productWindow = new Window("Produkter", 2, 10, productList);
        productWindow.Draw();
    }

    public static void ShowDetails(int productId)
    {
        Console.Clear();
        HeaderView.Show();
        using var db = new WebStoreContext();
        var product = db.Products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            Console.WriteLine("Produkten hittades inte!");
            return;
        }

        var details = new List<string>
        {
            $"Namn: {product.Name}",
            $"Beskrivning: {product.Description ?? "Ingen beskrivning"}",
            $"Pris: {product.Price} kr",
            $"Leverantör: {product.Supplier ?? "Okänd"}",
            "Tryck B för att köpa",
            "Tryck 9 för att gå tillbaka till menyn"
        };

        var detailWindow = new Window($"Info om {product.Name}", 2, 10, details);
        detailWindow.Draw();
    }

    public static int BuyProductView(int productId)
    {
        using var db = new WebStoreContext();

        var product = db.Products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            var errorWindow = new Window("Fel", 2, 18, new List<string> { "Produkten hittades inte!" });
            errorWindow.Draw();
            return 0;
        }

        int quantity = 0;
        bool validInput = false;

        while (!validInput)
        {
            var buyWindow = new Window($"Antal av: {product.Name}", 2, 18, new List<string> { "Ange antal: " });
            buyWindow.Draw();

            Console.SetCursorPosition(17, 19);
            var input = Console.ReadLine();
            if (int.TryParse(input, out quantity) && quantity > 0)
            {
                validInput = true;
                
                if (product.StockQuantity < quantity)
                {
                    StockErrorView(product, quantity);
                    Console.ReadKey();
                    return 0;
                }
            }
            else
            {
                buyWindow = new Window("Fel antal", 2, 21, new List<string> { "Du måste ange minst 1" });
                buyWindow.Draw();
            }
        }

        var confirmationWindow = new Window("Tillagd i varukorgen", 2, 10,
            new List<string>
            {
                $"{quantity} av varan {product.Name} har lagts till i varukorgen",
                "Tryck valfri knapp för att gå vidare..."
            });
        Console.Clear();
        HeaderView.Show();
        confirmationWindow.Draw();
        Console.ReadKey(true);

        return quantity;
    }

    private static void StockErrorView(Product product, int quantity)
    {
        var rows = new List<string>();
        
        if (product.StockQuantity < 1)
        {
            rows.Add($"{product.Name} är tyvärr slut");
            rows.Add($"Tryck valfri knapp för att gå vidare...");
        }
        else if (product.StockQuantity < quantity)
        {
            rows.Add($"Tyvärr, det finns endast {product.StockQuantity} kvar av {product.Name}, välj ett lägre antal.");
            rows.Add($"Tryck valfri knapp för att gå vidare...");
        }

        var errorWindow = new Window("Fel", 2, 18, rows);
        errorWindow.Draw();
    }
}