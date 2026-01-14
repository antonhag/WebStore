using WebStore.Data;

namespace WebStore.GUI;

public class ProductListView
{
    public static void Show(int categoryId)
    {
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
        
        var productWindow = new Window ("Produkter", 2, 10, productList);
        productWindow.Draw();
    }
    
    public static void ShowDetails(int productId)
    {
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
            "Tryck 9 för att gå tillbaka"
        };
        
        var detailWindow = new Window ($"Info om {product.Name}", 2, 10, details);
        detailWindow.Draw();
    }
}