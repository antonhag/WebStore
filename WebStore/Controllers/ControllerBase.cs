using WebStore.Data;

namespace WebStore.Controllers;

public abstract class ControllerBase
{
    public async Task RunAsync()
    {
        bool running = true;

        while (running)
        {
            try
            {
                Console.Clear();
                await DrawViewAsync();
                running = await HandleInputAsync();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ett fel inträffade: ");
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
    
    protected abstract Task DrawViewAsync();
    protected abstract Task<bool> HandleInputAsync();

    public virtual void ShowError(string message)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

}