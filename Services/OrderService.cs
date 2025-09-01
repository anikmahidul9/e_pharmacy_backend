using e_pharmacy.Models;
using e_pharmacy.Services;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace e_pharmacy.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly ProductService _productService;

        public OrderService(MongoDbContext context, ProductService productService)
        {
            _orders = context.Orders;
            _productService = productService;
        }

        public async Task<Order> CreateOrderAsync(Cart cart)
        {
            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var item in cart.Items)
            {
                var product = await _productService.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    };
                    orderItems.Add(orderItem);
                    totalPrice += product.Price * item.Quantity;
                }
            }

            var order = new Order
            {
                UserId = cart.UserId,
                Items = orderItems,
                TotalPrice = totalPrice
            };

            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orders.Find(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }
    }
}
