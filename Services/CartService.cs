using e_pharmacy.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Linq;

namespace e_pharmacy.Services
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _carts;
        private readonly ProductService _productService;

        public CartService(MongoDbContext context, ProductService productService)
        {
            _carts = context.Carts;
            _productService = productService;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            var cart = await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            if (cart != null)
            {
                await CalculateTotalPrice(cart);
            }
            return cart;
        }

        public async Task<Cart> AddToCartAsync(string userId, string productId, int quantity)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _carts.InsertOneAsync(cart);
            }

            var cartItem = cart.Items.Find(i => i.ProductId == productId);

            if (cartItem == null)
            {
                var item = new e_pharmacy.Models.CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ProductName = "",
                    ProductPrice = 0,
                    ProductImage = ""
                };
                cart.Items.Add(item);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await CalculateTotalPrice(cart);
            await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return cart;
        }

        public async Task<Cart?> RemoveFromCartAsync(string userId, string productId)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart != null)
            {
                cart.Items.RemoveAll(i => i.ProductId == productId);
                await CalculateTotalPrice(cart);
                await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }

            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart != null)
            {
                cart.Items.Clear();
                await CalculateTotalPrice(cart);
                await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }
        }

        private async Task CalculateTotalPrice(Cart cart)
        {
            decimal total = 0;
            foreach (var item in cart.Items)
            {
                var product = await _productService.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Title;
                    item.ProductPrice = product.Price;
                    item.ProductImage = product.Image;
                    total += product.Price * item.Quantity;
                }
            }
            cart.TotalPrice = total;
        }
    }
}
