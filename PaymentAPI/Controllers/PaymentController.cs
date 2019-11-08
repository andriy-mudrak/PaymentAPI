using System;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<TransactionModel>> Post([FromQuery]string type, [FromBody] PaymentModel payment)
        {
            if (RequestTypeValidator.TypeValidation(type, payment))
            {
                var paymentType = RequestTypeValidator.PaymentChecker(type);
                if (paymentType == PaymentServiceConstants.PaymentType.Default) return BadRequest("Please check type of your entity");

                var response = _paymentService.Pay(paymentType, payment);
                return Ok(response);
            }
            else return BadRequest("Please check your entity");
        }

        [HttpGet]
        public IActionResult Get([FromQuery]int orderId = 0, [FromQuery] int userId = 0, [FromQuery] int vendorId = 0,
            [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            //if (type != null && id == 0 || type == null && id != 0) return BadRequest("Please enter type and id");
            //else
            //{

            //}
            return Ok(_paymentService.GetTransactions(orderId, userId, vendorId, startDate, endDate));
        }
    }
}
