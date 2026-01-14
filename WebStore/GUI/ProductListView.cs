using WebStore.Data;

namespace WebStore.GUI;

public class ProductListView
{
    public static void Show(int categoryId)
    {
        var productList = new List<string>();
        
        using (var db = new WebStoreContext())
        {
            var products = db.Products.Where(p => p.CategoryId == categoryId).ToList();

            int index = 1;
            
            foreach (var product in products)
            {
                productList.Add($"{index}. {product.Name} - {product.Price} kr");
                index++;
            }
        }
        
        var productWindow = new Window ("Produkter", 2, 10, productList);
        productWindow.Draw();
        
}
}