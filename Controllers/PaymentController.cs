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
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public PaymentController(PaymentService paymentService, CartService cartService, OrderService orderService)
        {
            _paymentService = paymentService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var cart = await _cartService.GetByUserIdAsync(userId);

            if (cart == null || cart.Items.Count == 0)
            {
                return BadRequest("Cart is empty.");
            }

            // In a real application, you would calculate the total amount here
            // and pass it to the payment service.
            var paymentSuccess = await _paymentService.ProcessPaymentAsync(userId, 0);

            if (paymentSuccess)
            {
                var order = await _orderService.CreateOrderAsync(cart);
                await _cartService.ClearCartAsync(userId);
                return Ok(order);
            }

            return BadRequest("Payment failed.");
        }
    }
}
