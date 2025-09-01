using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace e_pharmacy.Models
{
    public class CartItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; } = null!;
    }
}