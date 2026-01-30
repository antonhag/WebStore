using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.GUI;
using WebStore.Models;

namespace WebStore.Controllers;

public class LoginController : ControllerBase
{
    protected override async Task DrawViewAsync()
    {
        LoginView.Show();
    }

    protected override async Task<bool> HandleInputAsync()
    {
        try
        {
            var key = Console.ReadKey(true).KeyChar;

            switch (key)
            {
                case '1':
                    await LoginCustomerAsync();
                    return true;
                case '2':
                    await NewCustomerAsync();
                    return true;
                case '3':
                    await LoginAdminAsync();
                    return true;
                case '9':
                    return false;
                default:
                    ShowError("Ogiltigt val!");
                    return true;

            }
        }
        catch (Exception ex)
        {
            ShowError($"Något gick fel: {ex.Message}");
            return true;
        }
    }

    private async Task LoginCustomerAsync()
    {
        LoginView.ShowLoginCustomer();
        
        using var db = new WebStoreContext();
        
        var (email, password) = LoginView.GetCredentials();

        var customer = await db.Customers.Include(c=> c.City).ThenInclude(ci => ci.Country).Include(c => c.CreditCards).FirstOrDefaultAsync(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            Session.CurrentCustomer = customer;
            var homeController = new CustomerController();
            await homeController.RunAsync();
        }
        else
        {
            ShowError("Användaren hittades ej!");
        }
    }

    private async Task LoginAdminAsync()
    {
        LoginView.ShowLoginAdmin();
        
        using var db = new WebStoreContext();
        
        var (username, password) = LoginView.GetCredentials();
        
        var admin = await db.Admins.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);

        if (admin != null)
        {
            var adminController = new AdminController();
            await adminController.RunAsync();
        }
        else
        {
            ShowError("Adminstratören hittades ej!");
        }
    }

    private async Task NewCustomerAsync()
    {
        var cities = await GetCitiesAsync();
        
        var (firstName, lastName, phoneNumber, email, password, birthDate, street, zipCode, cityId) = LoginView.NewCustomerView(cities);
        
        using var db = new WebStoreContext();
        var newCustomer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password,
            BirthDate = birthDate,
            Street = street,
            ZipCode = zipCode,
            CityId = cityId
        };
        
        db.Customers.Add(newCustomer);
        await db.SaveChangesAsync();
        
        Console.WriteLine("Kunden har skapats!");
        Console.WriteLine("Tryck valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }
    
    private async Task<List<City>> GetCitiesAsync()
    {
        try
        {
            using var db = new WebStoreContext();
            var cities = await db.Cities.ToListAsync();
            return cities;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fel i GetCitiesAsync: " + ex.Message);
            return new List<City>();
        }
    }
}