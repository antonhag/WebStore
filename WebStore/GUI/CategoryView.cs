using Dapper;
using Microsoft.Data.SqlClient;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.GUI;

public class CategoryView
{
    public static async Task ShowAsync()
    {
        var categoryList = new List<string>();
        
        var connectionString = "Server=localhost,14330;Database=WebStoreDb;User Id=sa;Password=StrongP@ssw0rd!;TrustServerCertificate=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "SELECT Id, Name FROM webstore.Categories";
            
            var categories = await connection.QueryAsync<Category>(sql);

            foreach (var category in categories)
            {
                categoryList.Add($"{category.Id}. {category.Name}");
            }
        }
        var categoryWindow = new Window ("Kategorier", 2, 10, categoryList);
        
        categoryWindow.Draw();
    }
}