using Microsoft.Extensions.Configuration;

namespace WebStore.Helpers;

public static class ConnectionStringHelper
{
    private static readonly IConfiguration _config;

    // Statisk konstruktor – måste ha samma namn som klassen
    static ConnectionStringHelper()
    {
        _config = new ConfigurationBuilder()
            .AddUserSecrets<UserSecretsMarker>() // kopplar till UserSecretsId
            .Build();
    }

    public static string GetSqlConnectionString(string name = "WebStoreDb")
    {
        return _config.GetConnectionString(name);

    }

    public static string GetMongoConnectionString(string name = "MongoDb")
    {
        return _config.GetConnectionString(name);
    }
}

public class UserSecretsMarker{ } // För att AddUserSecrets ska fungera så kräver den en icke-statisk klass