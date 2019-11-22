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
    public class PaymentAuthentication : IPaymentExecute
    {
        public PaymentAuthentication(IPaymentRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper) 
            : base(paymentRepository, mappingProvider, retryHelper)
        {
        }

        public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            UserDTO user;
            user = await _paymentRepository.GetUserByEmail(payment.Email);
            user = user ?? await CreateUser(payment);
            
            var options = new ChargeCreateOptions
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                Customer = user.ExternalId,
                Capture = false,
            };
            var service = new ChargeService();

            var transaction = await _retryHelper.RetryIfThrown(async () =>
            {
                var result = await service.CreateAsync(options);
                return _mappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.Stripe_Succeeded)
                    .Map(PaymentServiceConstants.PaymentType.Auth, payment, result, result.Created);

            }, PaymentServiceConstants.PaymentType.Auth, payment, PaymentServiceConstants.isSucceeded.Succeeded);

            return await _paymentRepository.CreateTransactions(transaction);
        }
    }
}