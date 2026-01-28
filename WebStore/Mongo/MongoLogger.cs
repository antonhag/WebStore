using MongoDB.Driver;
using WebStore.Models;
using WebStore.Mongo;

namespace WebStore.Helpers;

public class MongoLogger
{
    // Metod för att spara sökning
    public static async Task LogProductSearchAsync(string textSearched, Customer? customer)
    {
        var collection = MongoDbContext.GetProductSearchLog();

        var log = new ProductSearchLog
        {
            TextSearched = textSearched,
            Customer = new CustomerInfo
            {
                CustomerId = customer.Id,
                Name = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email
            }
        };

        await collection.InsertOneAsync(log);
    }

    public static async Task<List<ProductSearchLog>> GetSearchHistoryAsync()
    {
        var collection = MongoDbContext.GetProductSearchLog();
        
        var allSearches = await collection.Find(_ => true).ToListAsync();
        return allSearches;
    }
}