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
    public class PaymentCapture : PaymentExecuteBase
    {
        private readonly ITransactionQueryCreator _queryCreator;
        public PaymentCapture(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper, ITransactionQueryCreator queryCreator)
            : base(paymentRepository, mappingProvider, retryHelper)
        {
            _queryCreator = queryCreator;
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = (await PaymentRepository.GetTransactions(await _queryCreator.GetTransactionForCapture(payment.OrderId, PaymentServiceConstants.PaymentType.Auth))).LastOrDefault();

            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId, Amount = customer.Amount };

            var action = new Func<Task<TransactionDTO>>(
                async () =>
                {
                    var result = await service.CaptureAsync(customer.ExternalId, null);

                    return MappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.StripeSucceeded)
                        .Map(PaymentServiceConstants.PaymentType.Capture, orderInfo, result, result.Created);

                });

            return await ExecuteAndSave(action, PaymentServiceConstants.PaymentType.Capture, payment,
                PaymentServiceConstants.IsSucceeded.Succeeded);
        }
    }
}