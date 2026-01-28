using MongoDB.Driver;

namespace WebStore.Mongo;

public class MongoDbContext
{
    private static MongoClient GetClient()
    {
        string connectionString =
            "mongodb+srv://antonhagstrom_db_user:AFi2YJPedup3uDm7@webstore.gye6cg2.mongodb.net/?appName=Webstore";

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