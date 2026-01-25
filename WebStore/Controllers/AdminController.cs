using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class AdminController : ControllerBase
{
    protected override void DrawView()
    {
        AdminView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey().KeyChar;

        switch (key)
        {
            case '1':
                ManageProducts();
                return true;
            case '2':
                return true;
            case '3':
                return true;
            case '4':
                ManageSelectedProducts();
                return true;
            case '5':
                return true;
            case '9':
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

            Console.SetCursorPosition(21, 31);
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

            if (product == null)
            {
                ShowError("Produkten hittades inte!");
                continue;
            }

            int selectedCount = db.Products.Count(p => p.SelectedProduct); // Kollar hur många produkter som är selected
            if (product.SelectedProduct == false &&
                selectedCount >= 3) // Ifall användaren försöker markera fler än 3 produkter, gör detta
            {
                ShowError("Endast 3 produkter kan markeras som utvalda");
                continue;
            }

            product.SelectedProduct = !product.SelectedProduct; // Sätter den till motsatsen av vad den är nu
            db.SaveChanges();
        }
    }

    private void ManageProducts()
    {
        AdminView.ShowAllProducts();

        Console.SetCursorPosition(21, 31);
        var input = Console.ReadLine();

        if (input?.ToUpper() == "B") // För att gå tillbaka till menyn
        {
            return;
        }

        if (input?.ToUpper() == "A")
        {
            AddProduct();
            return;
        }

        if (!int.TryParse(input, out int productId))
        {
            ShowError("Ogiltigt ID! Ange endast siffror!");
            return;
        }

        using var db = new WebStoreContext();
        var product = db.Products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            ShowError("Produkten hittades inte!");
            return;
        }


        Console.Clear();
        AdminView.ManageProductsView();


        var optionInput = Console.ReadKey(true).KeyChar;

        switch (optionInput)
        {
            case '1':
                ChangeProduct(product);
                break;
            case '2':
                DeleteProduct(product);
                break;
            case '9':
                break;
            default:
                ShowError("Ogiltigt val!");
                break;
        }
    }

    private static void ChangeProduct(Product product)
    {
        var result = AdminView.ChangeProductView(product);
        
        // 2. Uppdatera endast om användaren angav något
        if (!string.IsNullOrWhiteSpace(result.productName))
            product.Name = result.productName;

        if (!string.IsNullOrWhiteSpace(result.description))
            product.Description = result.description;

        if (result.price.HasValue)
            product.Price = result.price.Value;

        if (result.productCategory.HasValue)
            product.CategoryId = result.productCategory.Value;

        if (!string.IsNullOrWhiteSpace(result.supplier))
            product.Supplier = result.supplier;

        if (result.stockQuantity.HasValue)
            product.StockQuantity = result.stockQuantity.Value;
        
        using var db = new WebStoreContext();
        db.Products.Update(product);
        db.SaveChanges();
        
        
        Console.WriteLine("Produkten uppdaterades!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }

    private static void DeleteProduct(Product product)
    {
        while (true)
        {
            var userInput = AdminView.DeleteProductView(product);
            
            if (userInput == 'J')
            {
                using var db = new WebStoreContext();
                db.Products.Remove(product);
                db.SaveChanges();
                Console.WriteLine($"{product.Name} har tagits bort!");
                Console.WriteLine("Tryck valfri tangent för att fortsätta...");
                Console.ReadKey(true);
                break;
            }

            else if (userInput == 'N')
            {
                break;
            }
            else
            {
                Console.WriteLine("Ogiltigt val!");
            }
        }
    }

    private static void AddProduct()
    {
        var productInfo = AdminView.AddProductView();

        Product newProduct = new Product
        {
            Name = productInfo.productName,
            Description = productInfo.description,
            Price = productInfo.price,
            CategoryId = productInfo.productCategory,
            Supplier = productInfo.supplier,
            StockQuantity = productInfo.stockQuantity
        };
        
        using var db = new WebStoreContext();
        
        var categoryExists = db.Categories.Any(c => c.Id == newProduct.CategoryId);
        
        db.Products.Add(newProduct);
        db.SaveChanges();

        Console.WriteLine($"Produkt {newProduct.Name} har lagts till!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }
}