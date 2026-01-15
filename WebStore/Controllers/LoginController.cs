using WebStore.Data;
using WebStore.GUI;

namespace WebStore.Controllers;

public class LoginController : ControllerBase
{
    protected override void DrawView()
    {
        //HeaderView.Show();
        LoginView.Show();
    }

    protected override bool HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                LoginCustomer(); 
                return true;
            case ConsoleKey.D2:
                new AdminController().Run();
                return true;
            case ConsoleKey.D9:
                Environment.Exit(0);
                return true;
            default:
                ShowError("Ogiltigt val!");
                return true;
                
        }
    }

    private void LoginCustomer()
    {
        LoginView.ShowLogin();
        
        using var db = new WebStoreContext();
        
        var (email, password) = LoginView.GetCredentials();

        var customer = db.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            Session.CurrentCustomer = customer;
            var homeController = new HomeController();
            homeController.Run();
        }
        else
        {
            ShowError("Användaren hittades ej!");
        }
    }
}