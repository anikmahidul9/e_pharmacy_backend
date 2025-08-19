using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace e_pharmacy.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();
    }
}