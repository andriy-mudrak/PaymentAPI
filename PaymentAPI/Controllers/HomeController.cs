using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PaymentAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly string PublishableKey;
        private readonly IPaymentService _paymentService;

        public HomeController(IConfiguration configuration, IPaymentService paymentService)
        {
            PublishableKey = configuration.GetSection("Stripe:PublishableKey").Value;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            ViewBag.StripePublishableKey = PublishableKey;
            return View();
        }

        public ActionResult Charge(string stripeEmail, string stripeToken, int stripeAmount)
        {
            var payment = new PaymentModel
            {
                CardToken = stripeToken,
                Amount = 100,
                Email = stripeEmail,
                OrderId = 1,
                UserId = 1,
                SaveCard = true,
                VendorId = 1,
                Currency = "usd"
            };
            _paymentService.Pay(PaymentServiceConstants.CHARGE, payment);

            return Ok();
        }

    }
}
