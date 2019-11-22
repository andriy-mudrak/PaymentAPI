using System;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BLL.Helpers;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string type, [FromBody] PaymentModel payment)
        {
            using (LogContext.PushProperty("OrderId", payment.OrderId))
            using (LogContext.PushProperty("UserId", payment.UserId))
            using (LogContext.PushProperty("VendorId", payment.VendorId))
            using (LogContext.PushProperty("Type", type))
            using (LogContext.PushProperty("Amount", payment.Amount))
            {
                Log.Information($"Payment info {JsonConvert.SerializeObject(payment)}");
                if (RequestTypeValidator.TypeValidation(type, payment))
                {
                    var paymentType = RequestTypeValidator.PaymentChecker(type);
                    if (paymentType == PaymentServiceConstants.PaymentType.Default)
                        return BadRequest("Please check type of your entity");

                    return Ok(await _paymentService.Pay(paymentType, payment));
                }
                else return BadRequest("Please check your entity");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int orderId = 0, [FromQuery] int userId = 0, [FromQuery] int vendorId = 0,
            [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            return Ok(await _paymentService.GetTransactions(orderId, userId, vendorId, startDate, endDate));
        }
    }
}
