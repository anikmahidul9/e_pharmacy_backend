using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace e_pharmacy.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending"; // e.g., Pending, Paid, Shipped, Delivered

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}