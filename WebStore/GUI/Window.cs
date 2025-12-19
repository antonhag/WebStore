namespace WebStore.GUI;

public class Window
{
    public string Header { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
    public List<string> TextRows { get; set; }

    public Window(string header, int left, int top, List<string> textRows)
    {
        Header = header;
        Left = left;
        Top = top;
        TextRows = textRows;
    }

    public void Draw()
    {
        // Hitta bredden baserat på längsta raden eller headern
        var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault()?.Length ?? 0;
        if (width < Header.Length + 4)
        {
            width = Header.Length + 4;
        }

        // Sätt färg för hela ramen och texten
        Console.ForegroundColor = ConsoleColor.Red; // Samma som erbjudandetexten

        // Rita Header
        Console.SetCursorPosition(Left, Top);
        if (!string.IsNullOrEmpty(Header))
        {
            Console.Write('┌' + " ");
            Console.ForegroundColor = ConsoleColor.Cyan; // Header färg
            Console.Write(Header);
            Console.ForegroundColor = ConsoleColor.Red; // Ramen tillbaka till samma färg som texten
            Console.Write(" " + new string('─', width - Header.Length) + '┐');
        }
        else
        {
            Console.Write('┌' + new string('─', width + 2) + '┐');
        }

        // Rita text-rader med ramen
        for (int j = 0; j < TextRows.Count; j++)
        {
            Console.SetCursorPosition(Left, Top + j + 1);
            Console.Write('│' + " " + TextRows[j] + new string(' ', width - TextRows[j].Length + 1) + '│');
        }

        // Rita nederdel av fönstret
        Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
        Console.Write('└' + new string('─', width + 2) + '┘');

        // Uppdatera lägsta position
        if (Lowest.LowestPosition < Top + TextRows.Count + 2)
        {
            Lowest.LowestPosition = Top + TextRows.Count + 2;
        }

        // Återställ färger
        Console.ResetColor();

        Console.SetCursorPosition(0, Lowest.LowestPosition);
    }

}

public static class Lowest
{
    public static int LowestPosition { get; set; }
}