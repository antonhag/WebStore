using WebStore.Controllers;
using WebStore.GUI;

namespace WebStore;

public static class Shop
{
    public static void Run()
    {
        var loginController = new LoginController();
        loginController.Run();
        //homeController.Run();
    }
}