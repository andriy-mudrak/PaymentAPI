using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Repositories.Interfaces;
using DAL.DBModels;
using Stripe;

namespace BLL.Services
{
    public class PaymentCapture : IPaymentExecute
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentCapture(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = _paymentRepository.GetTransactions(payment.OrderId, payment.UserId).Result.LastOrDefault();
            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId };

            if (!customer.TransactionType.Equals(PaymentServiceConstants.AUTH)) throw new Exception("Server can not find auth transaction");//TODO: треба ще ексепшин хендлінг

            var response = RetryHelpers.RetryIfThrown(async () =>
            {
                var result = await service.CaptureAsync(customer.ExternalId, null);
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.STRIPE_SUCCEEDED]
                    .Map(PaymentServiceConstants.CAPTURE, orderInfo, result, result.Created);

            }, PaymentServiceConstants.CAPTURE, payment, PaymentServiceConstants.STRIPE_SUCCEEDED);

            return await _paymentRepository.CreateTransaction(response);
        }
    }
}