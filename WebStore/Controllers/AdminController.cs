using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class AdminController : ControllerBase
{
    protected override async Task DrawViewAsync()
    {
        AdminView.Show();
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            var key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '1':
                    await ManageProductsAsync();
                    return true;
                case '2':
                    await ManageCategoriesAsync();
                    return true;
                case '3':
                    await ManageCustomerAsync();
                    return true;
                case '4':
                    await ManageSelectedProductsAsync();
                    return true;
                case '5':
                    var statsController = new StatsController();
                    await statsController.RunAsync();
                    return true;
                case '9':
                    Session.Logout();
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

    private async Task ManageSelectedProductsAsync()
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
                continue;
            }

            using var db = new WebStoreContext();
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                ShowError("Produkten hittades inte!");
                continue;
            }

            int selectedCount = await db.Products.CountAsync(p => p.SelectedProduct); // Kollar hur många produkter som är selected
            if (product.SelectedProduct == false &&
                selectedCount >= 3) // Ifall användaren försöker markera fler än 3 produkter, gör detta
            {
                ShowError("Endast 3 produkter kan markeras som utvalda");
                continue;
            }

            product.SelectedProduct = !product.SelectedProduct; // Sätter den till motsatsen av vad den är nu
            await db.SaveChangesAsync();
        }
    }

    private async Task ManageProductsAsync()
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
            await AddProductAsync();
            return;
        }

        if (!int.TryParse(input, out int productId))
        {
            ShowError("Ogiltigt ID! Ange endast siffror!");
            return;
        }

        using var db = new WebStoreContext();
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);

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
                await ChangeProductAsync(product);
                break;
            case '2':
                await DeleteProductAsync(product);
                break;
            case '9':
                break;
            default:
                ShowError("Ogiltigt val!");
                break;
        }
    }

    private async Task ChangeProductAsync(Product product)
    {
        var result = AdminView.ChangeProductView(product);
        
        // Uppdatera endast om användaren angav något
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
        await db.SaveChangesAsync();
        
        
        Console.WriteLine("Produkten uppdaterades!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }

    private async Task DeleteProductAsync(Product product)
    {
        while (true)
        {
            var userInput = AdminView.DeleteProductView(product);
            
            if (userInput == 'J')
            {
                using var db = new WebStoreContext();
                db.Products.Remove(product);
                await db.SaveChangesAsync();
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

    private async Task AddProductAsync()
    {
        var productInfo = await AdminView.AddProductView();

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
        
        var categoryExists = await db.Categories.AnyAsync(c => c.Id == newProduct.CategoryId);
        
        db.Products.Add(newProduct);
        await db.SaveChangesAsync();

        Console.WriteLine($"Produkt {newProduct.Name} har lagts till!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }

    private async Task ManageCategoriesAsync()
    {
        try
        {
            AdminView.ManageCategoriesView();

            var key = Console.ReadKey(true).KeyChar;

            switch (key)
            {
                case '1':
                    await AddCategoryAsync();
                    break;
                case '2':
                    await DeleteCategoryAsync();
                    break;
                case '3':
                    await ChangeCategoryAsync();
                    break;
                case '9':
                    break;
                default:
                    ShowError("Ogiltigt val!");
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowError("Ett fel inträffade: {ex.Message}");
        }
     
    }

    private async Task AddCategoryAsync()
    {
        var categoryName = AdminView.AddCategoryView();
        
        using var db = new WebStoreContext();

        Category newCategory = new Category
        {
            Name = categoryName
        };
        
        db.Categories.Add(newCategory);
        await db.SaveChangesAsync();

        Console.Clear();
        Console.WriteLine($"Kategorin {newCategory.Name} har lagts till!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);    }

    private async Task DeleteCategoryAsync()
    {
        var chosenId = await AdminView.DeleteCategoryViewAsync();
        
        using var db = new WebStoreContext();
        
        var categoryToDelete = await db.Categories.FirstOrDefaultAsync(c => c.Id == chosenId);
        
        db.Categories.Remove(categoryToDelete);
        await db.SaveChangesAsync();
        
        Console.Clear();
        Console.WriteLine($"Kategorin {categoryToDelete.Name} har tagits bort!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }
    
    private async Task ChangeCategoryAsync()
    {
        var (categoryId, newName) = await AdminView.ChangeCategoryView();
        
        using var db = new WebStoreContext();
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        
        if (category == null)
        {
            Console.WriteLine("Kategorin hittades inte.");
            return;
        }
        
        category.Name = newName;
        await db.SaveChangesAsync();
        
        Console.Clear();
        Console.WriteLine($"Kategorin har uppdaterats!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }
    
    private async Task ManageCustomerAsync()
    {
        Console.Clear();
        AdminView.AllCustomers();
        
        var input = Console.ReadLine();
        
        if (input?.ToUpper() == "B") // För att gå tillbaka till menyn
        {
            return;
        }

        if (!int.TryParse(input, out int customerId))
        {
            ShowError("Ogiltigt ID! Ange endast siffror!");
            return;
        }
        
        using var db = new WebStoreContext();
        
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        
        if (customer == null)
        {
            ShowError("Kunden hittades inte!");
            return;
        }
        
        Console.Clear();
        AdminView.ManageCustomerView();
        

        var optionInput = Console.ReadKey(true).KeyChar;
        
        switch (optionInput)
        {
            case '1':
                await ChangeCustomerAsync(customer);
                break;
            case '2':
                await CustomerOrderHistoryAsync(customerId);
                break;
            case '9':
                break;
            default:
                ShowError("Ogiltigt val!");
                break;
        }
    }

    private async Task ChangeCustomerAsync(Customer customer)
    {
        var newCustomerInfo = AdminView.ChangeCustomerView(customer);
        
        //  Uppdatera endast om användaren angav något
        if (!string.IsNullOrWhiteSpace(newCustomerInfo.newEmail))
            customer.Email = newCustomerInfo.newEmail;

        if (!string.IsNullOrWhiteSpace(newCustomerInfo.newPhoneNumber))
            customer.PhoneNumber = newCustomerInfo.newPhoneNumber;

        if (!string.IsNullOrWhiteSpace(newCustomerInfo.newPassword))
            customer.Password = newCustomerInfo.newPassword;

        if (!string.IsNullOrWhiteSpace(newCustomerInfo.newStreet))
            customer.Street = newCustomerInfo.newStreet;
        
        using var db = new WebStoreContext();
        db.Customers.Update(customer);
        await db.SaveChangesAsync();
        
        Console.Clear();
        Console.WriteLine($"Kunduppgifterna har uppdaterats!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }

    private async Task CustomerOrderHistoryAsync(int customerId)
    {
        using var db = new WebStoreContext();
        
        var customer = await db.Customers.Include(c => c.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
        {
            ShowError("Kunden hittades inte!");
            return;
        }
        
        AdminView.CustomerOrderHistoryView(customer);
    }
}