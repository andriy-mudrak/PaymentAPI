using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Newtonsoft.Json;
using Stripe;

namespace BLL.Services
{
    public class PaymentCharge : IPaymentExecute
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentCharge(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var options = new ChargeCreateOptions
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                Source = payment.CardToken
            };
            var service = new ChargeService();

            var response =  RetryHelpers.RetryIfThrown(async () =>
            {
                var result =  await service.CreateAsync(options);
               
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Map(PaymentServiceConstants.CHARGE, payment, result, result.Created); 

            }, PaymentServiceConstants.CHARGE, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);

            return await _paymentRepository.CreateTransaction(response); 
        }
    }
}