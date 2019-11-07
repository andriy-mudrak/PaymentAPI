using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Stripe;

namespace BLL.Services
{
    public class PaymentAuthentication : IPaymentExecute
    {
        public async Task Execute(IPaymentRepository _paymentRepository, PaymentModel payment)
        {
            var options = new ChargeCreateOptions
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                Source = payment.CardToken,
                Capture = false,
            };
            var service = new ChargeService();

            var response = RetryHelpers.RetryIfThrown(() =>
            {
                var result = service.Create(options);
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Transaction(PaymentServiceConstants.AUTH, payment, result);

            }, PaymentServiceConstants.AUTH, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);
            await _paymentRepository.CreateTransaction(response);
        }
    }
}