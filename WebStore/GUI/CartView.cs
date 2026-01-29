using Microsoft.EntityFrameworkCore;
using WebStore.Controllers;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CartView
{
    public static void Show(Cart cart)
    {
        if (Session.CurrentCustomer == null)
        {
            Console.WriteLine("Ingen kund är inloggad");
            Console.ReadKey();
            return;
        }
        
        var rows = new List<string>();
        decimal total = 0;

        int index = 1;
        foreach (var item in cart.Items)
        {
            rows.Add($"{index}. {item.Product.Name}");
            rows.Add($"   Antal: {item.Quantity}  Pris: {item.TotalPrice} kr");
            total += item.TotalPrice;
            index++;
        }

        rows.Add("------------------------------------------");
        rows.Add($"Totalt: {total} kr");
        rows.Add("C. Gå till kassan");
        rows.Add("9. Gå tillbaka till menyn");

        var cartWindow = new Window("Varukorg | Ange Produkt-Id för att ändra antal", 2, 10, rows);
        cartWindow.Draw();
    }

    public static int ChangeQuantityView(Product product)
    {
        while (true)
        {
            Console.Clear();
            HeaderView.ShowShopName();

            var changeWindow = new Window($"Ändra antal av {product.Name}", 2, 14,
                new List<string> { "Ange nytt antal: ","0 = ta bort varan" });
            changeWindow.Draw();
        
            Console.SetCursorPosition(21, 15);
            string input = Console.ReadLine();
        
            if (int.TryParse(input, out int newQuantity) && newQuantity >= 0)
            {
                if (newQuantity > product.StockQuantity)
                {
                    ProductListView.StockErrorView(product, newQuantity);
                    Console.Read();
                    continue;
                }
                
                return newQuantity;
            }
            
            Console.Clear();
            HeaderView.ShowShopName();
            var inputError = new Window("Ogiltigt antal!", 2, 10,
                new List<string> { "Ange ett tal 0 eller större", "Tryck valfri knapp för att gå vidare..." });
            inputError.Draw();
            Console.ReadKey();
            
            
        }
        
    }
}