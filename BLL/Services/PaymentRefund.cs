using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentRefund : PaymentExecuteBase
    {
        private readonly ITransactionQueryCreator _queryCreator;
        public PaymentRefund(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper, ITransactionQueryCreator queryCreator)
            : base(paymentRepository, mappingProvider, retryHelper)
        {
            _queryCreator = queryCreator;
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = (await PaymentRepository.GetTransactions(await _queryCreator.GetTransactionForRefund(payment.OrderId))).LastOrDefault();

            var service = new RefundService();

            var options = new RefundCreateOptions
            {
                Charge = customer.ExternalId,
            };

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId, Amount = customer.Amount };

            var action = new Func<Task<TransactionDTO>>(
                async () =>
                {
                    var result = await service.CreateAsync(options);
                    var test = MappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.StripeRefund)
                        .Map(PaymentServiceConstants.PaymentType.Refund, orderInfo, result, result.Created);
                    return test;

                });

            return await ExecuteAndSave(action, PaymentServiceConstants.PaymentType.Refund, payment,
                PaymentServiceConstants.IsSucceeded.Succeeded);
        }
    }
}