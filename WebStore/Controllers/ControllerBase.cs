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
            catch (Exception)
            {
             ShowError("Ogiltigt val!");
            }
            
        }
    }
    
    protected abstract void DrawView();
    protected abstract bool HandleInput();

    protected virtual void ShowError(string message)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

}