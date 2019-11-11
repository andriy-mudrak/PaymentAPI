using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentAuthentication : IPaymentExecute
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentAuthentication(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var options = new ChargeCreateOptions
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                Source = payment.CardToken,
                Capture = false,
            };
            var service = new ChargeService();

            var response = RetryHelpers.RetryIfThrown(async () =>
            {
                var result = await service.CreateAsync(options);
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Map(PaymentServiceConstants.AUTH, payment, result, result.Created);

            }, PaymentServiceConstants.AUTH, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);

            return await _paymentRepository.CreateTransaction(response);
        }
    }
}