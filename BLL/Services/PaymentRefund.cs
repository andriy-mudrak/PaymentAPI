using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentRefund : IPaymentExecute
    {
        public PaymentRefund(IPaymentRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
            : base(paymentRepository, mappingProvider, retryHelper)
        {
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = await _paymentRepository.GetLastTransaction(payment.OrderId);

            var service = new RefundService();

            var options = new RefundCreateOptions
            {
                Charge = customer.ExternalId,
            };

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId, Amount = customer.Amount };

            var transaction = await _retryHelper.RetryIfThrown(async () =>
            {
                var result = await service.CreateAsync(options);
                var test = _mappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.Stripe_Refund)
                    .Map(PaymentServiceConstants.PaymentType.Refund, orderInfo, result, result.Created);
                return test;

            }, PaymentServiceConstants.PaymentType.Refund, orderInfo, PaymentServiceConstants.isSucceeded.Succeeded);

            return await _paymentRepository.CreateTransactions(transaction);
        }
    }
}