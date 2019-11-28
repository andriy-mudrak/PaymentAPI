using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentAuthentication : PaymentExecuteBase
    {
        private readonly IUserModifier _userModifier;

        public PaymentAuthentication(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper, 
            IUserModifier userModifier) 
            : base(paymentRepository, mappingProvider, retryHelper)
        {
            _userModifier=userModifier;
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var user = await _userModifier.GetOrCreateUser(payment);
            
            var options = new ChargeCreateOptions
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                Customer = user.ExternalId,
                Capture = false,
            };
            var service = new ChargeService();
            
            var action = new Func<Task<TransactionDTO>>(
                async () =>
                {
                    var result = await service.CreateAsync(options);

                    return MappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.StripeSucceeded)
                        .Map(PaymentServiceConstants.PaymentType.Auth, payment, result, result.Created);

                });

            return await ExecuteAndSave(action, PaymentServiceConstants.PaymentType.Auth, payment,
                PaymentServiceConstants.IsSucceeded.Succeeded);
        }
    }
}