using e_pharmacy.Models;
using e_pharmacy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace e_pharmacy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var cart = await _cartService.GetByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(CartItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var cart = await _cartService.AddToCartAsync(userId, item.ProductId, item.Quantity);
            return Ok(cart);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<Cart>> RemoveFromCart(string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var cart = await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
