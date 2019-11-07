using System;
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
        public IActionResult Post([FromQuery]string type, [FromBody] PaymentModel payment)
        {
            if (RequestTypeValidator.TypeValidation(type, payment))
            {
                _paymentService.Pay(type, payment);
                return Ok();
            }
            else return BadRequest("Please check your entity");
        }

        [HttpGet]
        public IActionResult Get([FromQuery]string type, [FromQuery] int id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (type != null && id == 0 || type == null && id != 0) return BadRequest("Please enter type and id");
            else
            {
                return Ok(_paymentService.GetTransactions(type, id, startDate, endDate));
            }
        }
    }
}
