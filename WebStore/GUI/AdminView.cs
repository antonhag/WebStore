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
    
    private enum ManageProductsMenu
    {
        Ändra_produkt = 1,
        Ta_bort_produkt,
        Återgå_till_menyn = 9
    }

    private enum ManageCategoriesMenu
    {
        Lägga_till_produktkategori = 1,
        Ta_bort_produktkategori,
        Ändra_produktkategori,
        Återgå_till_menyn = 9
    }

    private enum ManageCustomerMenu
    {
        Ändra_kunds_uppgifter = 1,
        Se_kunds_beställningshistorik,
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
            "A. För att lägga till produkt",
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
        Console.WriteLine("--------- Ändra produkt ---------\n");
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
        Console.Write("Ange ny kategori-Id: ");
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

    public static (string productName, string description, decimal price, int productCategory, string supplier, int
        stockQuantity) AddProductView()
    {
        using var db = new WebStoreContext();
        
        Console.Clear();
        Console.WriteLine("--------- Lägg till ny produkt ---------\n");

        Console.Write("Produktnamn: ");
        var name = Console.ReadLine()!;
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Produktnamn får inte vara tomt försök igen: ");
            name = Console.ReadLine();
        }

        Console.Write("Beskrivning: ");
        var desc = Console.ReadLine() ?? ""; // Om användaren inte skriver något (null), sätt desc till tom sträng
        
        decimal price;
        while (true)
        {
            Console.Write("Pris: ");
            if (decimal.TryParse(Console.ReadLine(), out price) && price > 0) // Break ifall det stämmer, annars skriv felmeddelande
                break;
            Console.WriteLine("Fel: Ange ett giltigt pris större än 0.");
        }

        Console.Clear();
        
        var categories = db.Categories.ToList();
        Console.WriteLine("Tillgängliga kategorier:");
        foreach (var c in categories)
        {
            Console.WriteLine($"{c.Id}: {c.Name}");
        }
        
        Console.Write("\nKategori-Id: ");
        int categoryId;
        while (true)
        {
            Console.Write("Välj kategori-id: ");
            if (!int.TryParse(Console.ReadLine(), out categoryId))
            {
                Console.WriteLine("Fel: Ange en siffra.");
                continue;
            }

            if (!db.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"Fel: Kategori med ID {categoryId} finns inte. Försök igen.");
                continue;
            }

            break; // giltigt kategori-id
        }

        Console.Write("Leverantör: ");
        var supplier = Console.ReadLine();

        Console.Write("Lagerantal: ");
        int stock;
        while (true)
        {
            Console.Write("Lagerantal: ");
            if (int.TryParse(Console.ReadLine(), out stock) && stock >= 0)
                break;
            Console.WriteLine("Fel: Lagerantal måste vara 0 eller högre.");
        }
        
        return (name, desc, price, categoryId, supplier, stock);
    }
    
    public static void ManageCategoriesView()
    {
        Console.Clear();
        
        var categoryOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(ManageCategoriesMenu)))
        {
            categoryOptions.Add($"{i}. {Enum.GetName(typeof(ManageCategoriesMenu), i).Replace("_", " ")}");
        }
        

        var categoryOption = new Window("Välj alternativ", 2, 2, categoryOptions);
        
        categoryOption.Draw();
    }

    public static string AddCategoryView()
    {
        Console.Clear();
        Console.WriteLine("--------- Lägga till produktkategori ---------\n");

        Console.Write("Kategorinamn: ");
        var name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Clear();
            Console.Write("Kategorinamn får inte vara tomt försök igen: ");
            name = Console.ReadLine();
        }
        
        return name;
    }

    public static int DeleteCategoryView()
    {
        Console.Clear();
        Console.WriteLine("--------- Ta bort produktkategori ---------\n");
        
        using var db = new WebStoreContext();
        
        var categories = db.Categories.ToList();

        foreach (var c in categories)
        {
            Console.WriteLine($"{c.Id}: {c.Name}");
        }

        int categoryId;
        while (true)
        {
            Console.Write("Välj kategori-id du vill radera: ");
            if (!int.TryParse(Console.ReadLine(), out categoryId))
            {
                Console.WriteLine("Fel: Ange en siffra.");
                continue;
            }

            if (!db.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"Fel: Kategori med ID {categoryId} finns inte. Försök igen.");
                continue;
            }

            break; // giltigt kategori-id
        }
        
        return categoryId;
    }

    public static (int categoryId, string newName) ChangeCategoryView()
    {
        Console.Clear();
        Console.WriteLine("--------- Ändra produktkategori ---------\n");
        
        using var db = new WebStoreContext();
        
        var categories = db.Categories.ToList();

        foreach (var c in categories)
        {
            Console.WriteLine($"{c.Id}: {c.Name}");
        }

        int categoryId;
        while (true)
        {
            Console.Write("Välj kategori-id du vill ändra: ");
            if (!int.TryParse(Console.ReadLine(), out categoryId))
            {
                Console.WriteLine("Fel: Ange en siffra.");
                continue;
            }

            if (!db.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"Fel: Kategori med ID {categoryId} finns inte. Försök igen.");
                continue;
            }

            break; // giltigt kategori-id
        }
        var categoryToChange = db.Categories.FirstOrDefault(c => c.Id == categoryId); // för att kunna se tidigare kategorinamn
        
        Console.Clear();
        Console.WriteLine($"Tidigare kategorinamn:  {categoryToChange.Name}");
        Console.Write("Ange nytt kategorinamn: ");
        var newName = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(newName))
        {
            Console.Write("Kategorinamn får inte vara tomt försök igen: ");
            newName = Console.ReadLine();
        }
        
        return (categoryId, newName);
    }

    public static void ManageCustomerView()
    {
        Console.Clear();
        
        var customerOptions = new List<string>();

        foreach (int i in Enum.GetValues(typeof(ManageCustomerMenu)))
        {
            customerOptions.Add($"{i}. {Enum.GetName(typeof(ManageCustomerMenu), i).Replace("_", " ")}");
        }
        
        var window = new Window("Admin || Välj alternativ", 2, 2, customerOptions);
        
        window.Draw();
    }

    public static void AllCustomers()
    {
        Console.Clear();
        Console.WriteLine("--------- Alla kunder ---------\n");
        
        using var db = new WebStoreContext();
        var customers = db.Customers.ToList();

        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Id}. {c.FirstName} {c.LastName}");
        }

        Console.WriteLine("-------------------------------");
        
        Console.WriteLine("\nB = Återgå till menyn");
        Console.Write("Ange kund-id för den du vill välja: ");
    }
    
    public static (string? newEmail, string? newPhoneNumber, string? newPassword, string? newStreet) ChangeCustomerView(Customer customer) // Tar in customer objekt för att kunna se tidigare mail, nummer osv
    {
        Console.Clear();
        Console.WriteLine("--------- Ändra kunduppgifter ---------\n");
        Console.WriteLine("Lämna fältet tomt för oförändrat\n");

        Console.WriteLine($"Tidigare email: {customer.Email}");
        Console.Write("Ange ny email: ");
        var newEmail = Console.ReadLine();
        
        Console.WriteLine($"Tidigare telefonnummer: {customer.PhoneNumber}");
        Console.Write("Ange nytt telefonnummer: ");
        var newPhoneNumber = Console.ReadLine();
        
        Console.WriteLine($"Tidigare lösenord: {customer.Password}");
        Console.Write("Ange nytt lösenord: ");
        var newPassword = Console.ReadLine();

        Console.WriteLine($"Tidigare adress: {customer.Street}");
        Console.Write("Ange ny gatuadress: ");
        var newStreet = Console.ReadLine();

        return (newEmail, newPhoneNumber, newPassword, newStreet);
    }
    
    public static void CustomerOrderHistoryView(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("--------- Orderhistorik ---------\n");
        Console.WriteLine($"Kund: {customer.FirstName} {customer.LastName}\n");

        if (!customer.Orders.Any())
        {
            Console.WriteLine("Kunden har inga ordrar.");
            Console.WriteLine("Tryck valfri knapp för att gå vidare...");
            Console.ReadKey(true);
        }

        foreach (var order in customer.Orders.OrderBy(o => o.OrderDate))
        {
            Console.WriteLine($"Order #{order.Id}");
            Console.WriteLine($"Datum: {order.OrderDate}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Totalt: {order.TotalAmount} kr");
            Console.WriteLine("Produkter: ");

            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"  - {item.Product.Name} x{item.Quantity} | {item.Product.Price} kr");
            }
            
            Console.WriteLine(new string('-', 40)); // Snyggt radbryt
        }
        
        Console.WriteLine("\nTryck valfri tangent för att gå tillbaka...");
        Console.ReadKey(true);
    }
}