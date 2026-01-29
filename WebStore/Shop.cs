using Microsoft.EntityFrameworkCore;
using WebStore.Controllers;
using WebStore.Data;
using WebStore.GUI;

namespace WebStore;

public static class Shop
{
    public static async Task RunAsync()
    {
        var loginController = new LoginController();
        await loginController.RunAsync();
    }
}