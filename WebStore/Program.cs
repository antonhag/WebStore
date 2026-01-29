using WebStore.Data;
using WebStore.GUI;

namespace WebStore;

class Program
{
    static async Task Main(string[] args)
    {
        await Shop.RunAsync();
    }
}