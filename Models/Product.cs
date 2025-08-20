using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace e_pharmacy.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Ingredients { get; set; } = null!;
        public string HowToUse { get; set; } = null!;
    }
}