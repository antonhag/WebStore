using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class AdminView
{
    private enum AdminMenu
    {
        Adminstrera_produkter = 1,
        Adminstrera_kategorier,
        Adminstrera_kunder,
        Ändra_utvalda_produkter,
        Se_statistik,
        Återgå_till_startsida = 9
    }
    
    public enum ManageProductsMenu
    {
        Ändra_produkt = 1,
        Ta_bort_produkt,
        Återgå_till_menyn = 9
    }

    public static void Show()
    {
        var adminOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(AdminMenu)))
        {
            adminOptions.Add($"{i}. {Enum.GetName(typeof(AdminMenu), i).Replace("_", " ")}");
        }
        
        var adminWindow = new Window("Admin || Välj alternativ", 2, 2, adminOptions);
        
        adminWindow.Draw();
    }

    public static void ShowAllProducts()
    {
        Console.Clear();
        var productList  = new List<string>();
        var info = new List<string>
        {
            "Ange Produkt-ID: ",
            "X = Utvald produkt",
            "B. Gå tillbaka till menyn"
        };

        using (var db = new WebStoreContext())
        {
            var products = db.Products.ToList();
            
            productList.Add(
                "ID".PadRight(10) +
                "Produkt".PadRight(25) +
                "Pris".PadRight(15) +
                "Utvald"
            );
            
            productList.Add(new string('-', 60));
            
            foreach (var product in products)
            {
                productList.Add(
                    product.Id.ToString().PadRight(10) +
                    product.Name.PadRight(25) +
                    ($"{product.Price} kr").PadRight(15) +
                    (product.SelectedProduct ? "   X" : "")
                );
            }
            
            var window =  new Window("Alla produkter", 2, 2, productList);
            window.Draw();
            
            var window2 = new Window("Meny", 2, 30, info);
            window2.Draw();
        }
    }

    public static void ManageProductsView()
    {
        Console.Clear();
        
        var productOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(ManageProductsMenu)))
        {
            productOptions.Add($"{i}. {Enum.GetName(typeof(ManageProductsMenu), i).Replace("_", " ")}");
        }
        

        var productOption = new Window("Välj alternativ", 2, 2, productOptions);
        
        productOption.Draw();
    }

    public static (string? productName,string? description, decimal? price, int? productCategory, string? supplier, int? stockQuantity) ChangeProductView(Product product)
    {
        Console.Clear();
        Console.WriteLine("Lämna fältet tomt för oförändrat\n");
        
        Console.WriteLine($"Tidigare produktnamn: {product.Name}");
        Console.Write("Ange nytt produktnamn: ");
        var newName = Console.ReadLine();
        
        Console.WriteLine($"Tidigare beskrivning: {product.Description}");
        Console.Write("Ange ny beskrivning: ");
        var newDesc = Console.ReadLine();
        
        Console.WriteLine($"Tidigare pris: {product.Price}");
        Console.Write("Ange nytt pris: ");
        var priceInput = Console.ReadLine();

        decimal? newPrice = null;
        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out var parsedPrice)) // Felhantering
        {
            newPrice = parsedPrice;
        }
        
        Console.WriteLine($"Tidigare kategori-id: {product.CategoryId}");
        Console.Write("Ange ny kategori-id (lämna tom för oförändrat): ");
        var categoryInput = Console.ReadLine();

        int? newCategory = null;
        if (!string.IsNullOrWhiteSpace(categoryInput) &&
            int.TryParse(categoryInput, out var parsedCategory))
        {
            newCategory = parsedCategory;
        }
        
        Console.WriteLine($"Tidigare leverantör: {product.Supplier}");
        Console.Write("Ange ny leverantör (lämna tom för oförändrat): ");
        var newSupplier = Console.ReadLine();
        
        Console.WriteLine($"Tidigare lagerantal: {product.StockQuantity}");
        Console.Write("Ange nytt lagerantal (lämna tom för oförändrat): ");
        var stockInput = Console.ReadLine();

        int? newStock = null;
        if (!string.IsNullOrWhiteSpace(stockInput) &&
            int.TryParse(stockInput, out var parsedStock))
        {
            newStock = parsedStock;
        }
        
        return (
            newName,
            newDesc,
            newPrice,
            newCategory,
            newSupplier,
            newStock
        );
    }

    public static char DeleteProductView(Product product)
    {
        Console.Clear();

        Console.WriteLine($"Är du säker på att du vill radera {product.Name}?\n Tryck (j/n)");
        var key = Console.ReadKey(true).KeyChar;
        char upperKey = char.ToUpper(key);
        
        return upperKey;
    }
}