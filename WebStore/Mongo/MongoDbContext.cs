using MongoDB.Driver;
using WebStore.Helpers;

namespace WebStore.Mongo;

public class MongoDbContext
{
    private static MongoClient GetClient()
    {
        string connectionString = ConnectionStringHelper.GetMongoConnectionString();

        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        
        var client = new MongoClient(settings);
        return client;
    }

    public static IMongoCollection<ProductSearchLog> GetProductSearchLog()
    {
        var client = GetClient();
        
        var dataBase = client.GetDatabase("WebStore");
        
        var searchLogCollection = dataBase.GetCollection<ProductSearchLog>("ProductSearchLog");
        
        return searchLogCollection;
    }
    
}