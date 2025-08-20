using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace e_pharmacy.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Occupation { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}