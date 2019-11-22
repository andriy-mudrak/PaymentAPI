using System;
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
    public class PaymentCapture : IPaymentExecute
    {
        public PaymentCapture(IPaymentRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
            : base(paymentRepository, mappingProvider, retryHelper)
        {
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = await _paymentRepository.GetLastTransaction(payment.OrderId, PaymentServiceConstants.PaymentType.Auth.ToString());

            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId, Amount = customer.Amount };

            var transaction = await _retryHelper.RetryIfThrown(async () =>
            {
                var result = await service.CaptureAsync(customer.ExternalId, null);
           
                return _mappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.Stripe_Succeeded)
                    .Map(PaymentServiceConstants.PaymentType.Capture, orderInfo, result, result.Created);

            }, PaymentServiceConstants.PaymentType.Capture, orderInfo, PaymentServiceConstants.isSucceeded.Succeeded);

            return await _paymentRepository.CreateTransactions(transaction);
        }
    }
}