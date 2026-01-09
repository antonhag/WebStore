using WebStore.Controllers;
using WebStore.GUI;

namespace WebStore;

public static class Shop
{
    public static void Run()
    {
        HomeController homeController = new HomeController();
        homeController.Run();
    }
}