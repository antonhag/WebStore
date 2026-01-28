namespace WebStore.GUI;

public class StatsView
{
    private enum StatsMenu
    {
        Bästa_kunder_per_region = 1,
        Topp_5_kunder_som_spenderat_mest,
        Populäraste_produkter_per_åldersgrupp,
        Antal_kunder_per_stad,
        Topp_5_mest_sålda_produkter,
        Försäljning_per_leverantör,
        Återgå_till_menyn = 9
    }

    public static void ShowMenu()
    {
        var statsMenu = new List<string>();

        foreach (int i in Enum.GetValues(typeof(StatsMenu)))
        {
            statsMenu.Add($"{i}. {Enum.GetName(typeof(StatsMenu), i).Replace("_", " ")}");
        }

        var statsMenuWindow = new Window("Admin || Välj alternativ", 2, 2, statsMenu);

        statsMenuWindow.Draw();
    }

    public static void ShowBestSwedishCustomerPerRegionView(Dictionary<string, int> data)
    {
        Console.WriteLine("Bästa kunder per region:\n");

        if (data.Count == 0)
        {
            Console.WriteLine("Inga kunder hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        foreach (var item in data)
        {
            Console.WriteLine($"{item.Key,-10} Sverige | Ordrar:  {item.Value}");
        }

        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

    public static void Show5BestSpendingCustomersView(Dictionary<string, decimal> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Inga kunder hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        Console.WriteLine("Kund\t\tTotalt spenderat");
        Console.WriteLine("------------------------------------");
        foreach (var kvp in data)
        {
            Console.WriteLine($"{kvp.Key,-20}{kvp.Value} kr");
        }


        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

    public static void ShowPopularProductsByAgeView(Dictionary<string, Dictionary<string, int>> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Inga produkter hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        // Loopar igenom alla åldergrupper (under 30 och 30 och äldre)
        foreach (var ageGroup in data)
        {
            Console.WriteLine($"=== {ageGroup.Key} ====");

            // Sorterar produkterna i åldersgruppen efter antal
            foreach (var product in ageGroup.Value.OrderByDescending(x => x.Value))
            {
                Console.WriteLine(
                    $"{product.Key,-25} {product.Value}"); // product.Key = produktens namn, product.value = totalt antal sålda av just den produkt
            }

            Console.WriteLine();
        }

        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

    public static void ShowCustomersCountPerCityView(Dictionary<string, int> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Inga kunder hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        Console.WriteLine("Antal kunder per stad:\n");

        foreach (var city in data.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{city.Key, -20} {city.Value}"); // city.Key = stadsnamn, city.Value = antal kunder
        }

        Console.WriteLine($"\nTotalt antal städer: {data.Count}");
        
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }

    public static void Show5MostPopularProductView(Dictionary<string, int> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Inga kunder hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        Console.WriteLine("Topp 5 mest sålda produkter\n");

        foreach (var product in data.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{product.Key, -25} {product.Value} sålda");
        }
        
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }
    
    public static void ShowSalesBySupplierView(Dictionary<string, int> data)
    {
        if (data.Count == 0)
        {
            Console.WriteLine("Inga försäljningsdata hittades.");
            Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
            Console.ReadKey(true);
        }

        Console.WriteLine("Försäljning per leverantör:\n");

        foreach (var supplier in data)
        {
            Console.WriteLine($"{supplier.Key,-30} {supplier.Value} st sålda");
        }
        
        
        Console.WriteLine("\nTryck valfri knapp för att fortsätta...");
        Console.ReadKey(true);
    }
}