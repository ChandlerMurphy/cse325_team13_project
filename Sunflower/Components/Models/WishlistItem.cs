using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class WishlistItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int PlantId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Family { get; set; }
    public string? Type { get; set; }
    public string? Watering { get; set; }
    public IEnumerable<string>? Sunlight { get; set; }
    public string? Maintenance { get; set; }
    public bool Medicinal { get; set; }
    public bool Poisonous_To_Humans { get; set; }
    public string? Description { get; set; }
}
