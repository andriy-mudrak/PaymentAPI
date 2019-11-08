using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Repositories.Interfaces;
using PaymentAPI.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentCapture : IPaymentExecute
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentCapture(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = _paymentRepository.GetTransactions(payment.OrderId, payment.UserId).Result.LastOrDefault();
            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId };

            var response = RetryHelpers.RetryIfThrown(async () =>
            {
                var responseResult = await service.CaptureAsync(customer.ExternalId, null);
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Map(PaymentServiceConstants.CAPTURE, orderInfo, responseResult);

            }, PaymentServiceConstants.CAPTURE, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);

            return await _paymentRepository.CreateTransaction(response);
        }
    }
}