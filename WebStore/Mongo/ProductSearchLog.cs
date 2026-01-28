using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebStore.Models;

namespace WebStore.Mongo;

public class ProductSearchLog
{
    // Unikt Id för varje loggpost
    public string Id { get; set; } =  Guid.NewGuid().ToString();

    public string TextSearched { get; set; } = null!;

    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;

    public CustomerInfo? Customer { get; set; }
}

[BsonIgnoreExtraElements]
public class CustomerInfo
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}