using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Sunflower.Data; // This allows MongoDbContext to see User and MongoDbSettings

public class MongoDbContext
{
    private readonly IMongoDatabase _db;

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.DatabaseName);
    }

    // This exposes the Users collection
    public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
}
