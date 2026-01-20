using WebStore.Data;

namespace WebStore.Controllers;

public abstract class ControllerBase
{
    public void Run()
    {
        bool running = true;

        while (running)
        {
            try
            {
                Console.Clear();
                DrawView();
                running = HandleInput();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val! BASE");
                Console.WriteLine("Felmeddelande:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException:");
                    Console.WriteLine(ex.InnerException.Message);
                }
                Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
                Console.ReadKey(true);
            }
        }
    }
    
    protected abstract void DrawView();
    protected abstract bool HandleInput();

    public virtual void ShowError(string message)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

}