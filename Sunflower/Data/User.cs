// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;

// namespace Sunflower.Data
// {
//     public class User
//     {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string Id { get; set; } = null!;  // Add = null! to suppress warning

//         public string Username { get; set; } = null!;
//         public string Email { get; set; } = null!;
//         public string PasswordHash { get; set; } = null!;
//     }
// }




using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sunflower.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "customer"; // default role
    }
}
