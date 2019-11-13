using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BLL.Helpers;

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
        public async Task<IActionResult> Post([FromQuery]string type, [FromBody] PaymentModel payment)
        {
            if (RequestTypeValidator.TypeValidation(type, payment))
            {
                var paymentType = RequestTypeValidator.PaymentChecker(type);
                if (paymentType == PaymentServiceConstants.PaymentType.Default) return BadRequest("Please check type of your entity");

                return Ok(await _paymentService.Pay(paymentType, payment));
            }
            else return BadRequest("Please check your entity");
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int orderId = 0, [FromQuery] int userId = 0, [FromQuery] int vendorId = 0,
            [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            return Ok(await _paymentService.GetTransactions(orderId, userId, vendorId, startDate, endDate));
        }
    }
}
