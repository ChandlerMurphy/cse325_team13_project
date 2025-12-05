using MongoDB.Driver;

public class WishlistService
{
    private readonly IMongoCollection<WishlistItem> _wishlist;

    public WishlistService(IConfiguration config)
    {
        // Read connection string from environment variable
        var conn = Environment.GetEnvironmentVariable("MONGO_URL");

        if (string.IsNullOrWhiteSpace(conn))
            throw new Exception("MongoDB connection string (MONGO_URL) is not set.");

        var client = new MongoClient(conn);

        var dbName = config["MongoDB:DatabaseName"] ?? "Sunflower";
        var db = client.GetDatabase(dbName);

        _wishlist = db.GetCollection<WishlistItem>("Wishlist");
    }

    public async Task AddToWishlistAsync(WishlistItem item)
    {
        await _wishlist.InsertOneAsync(item);
    }

    public async Task<List<WishlistItem>> GetWishlistAsync()
    {
        return await _wishlist.Find(_ => true).ToListAsync();
    }

    public async Task DeleteWishlistItemAsync(string id)
    {
        await _wishlist.DeleteOneAsync(w => w.Id == id);
    }

}
