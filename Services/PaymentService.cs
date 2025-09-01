using System.Threading.Tasks;

namespace e_pharmacy.Services
{
    public class PaymentService
    {
        public async Task<bool> ProcessPaymentAsync(string userId, decimal amount)
        {
            // In a real application, you would integrate with a payment gateway here.
            // For this example, we'll just simulate a successful payment.
            return await Task.FromResult(true);
        }
    }
}
