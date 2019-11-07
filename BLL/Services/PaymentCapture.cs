using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Repositories.Interfaces;
using Stripe;

namespace BLL.Services
{
    public class PaymentCapture : IPaymentExecute
    {
        public async Task Execute(IPaymentRepository _paymentRepository, PaymentModel payment)
        {
            var customer = _paymentRepository.GetTransactions(PaymentRepositoryConstants.TransactionSelectorType.OrderId, payment.OrderId).Result.LastOrDefault();
            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId };

            var response = RetryHelpers.RetryIfThrown(() =>
            {
                var responseResult = service.Capture(customer.ExternalId, null);
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Transaction(PaymentServiceConstants.CAPTURE, orderInfo, responseResult);

            }, PaymentServiceConstants.CAPTURE, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);
            await _paymentRepository.CreateTransaction(response);
        }
    }
}