using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rent_A_Car.MVC.Models;
using Stripe.Checkout;
using Stripe;

namespace Rent_A_Car.MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly StripeSettings _stripeSettings;

        public PaymentController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Məhsulu"
                        },
                        UnitAmount = 1000 // $10.00
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "https://yourwebsite.com/success",
                CancelUrl = "https://yourwebsite.com/cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }
        public IActionResult Odenis()
        {
            return View();
        }
    }
}