using WebStore.GUI;

namespace WebStore;

class Program
{
    static void Main(string[] args)
    {
        HomeView homeView = new HomeView();

        homeView.Show();

        Console.ReadKey();
    }
}