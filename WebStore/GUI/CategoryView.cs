namespace WebStore.GUI;

public class CategoryView
{
    public enum Categories
    {
        Tröjor = 1,
        Skjortor,
        Byxor,
        Skor,
        Återgå_till_menyn = 9
    }

    public static void Show()
    {
        var categoryList = new List<string>();

        foreach (int i in Enum.GetValues(typeof(Categories)))
        {
            categoryList.Add($"{i}. {Enum.GetName(typeof(Categories), i).Replace("_", " ")}");
        }

        var categoryWindow = new Window("Kategorier", 2, 10, categoryList);
        
        categoryWindow.Draw();
    }
}