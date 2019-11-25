using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
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
        private readonly IMappingProvider _mappingProvider;
        private readonly IRetryHelper _retryHelper;

        public PaymentCapture(IPaymentRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
        {
            _paymentRepository = paymentRepository;
            _mappingProvider = mappingProvider;
            _retryHelper = retryHelper;
        }

        public async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
        {
            var customer = await _paymentRepository.GetLastTransaction(payment.OrderId);

            var service = new ChargeService();

            var orderInfo = new PaymentModel { UserId = customer.UserId, VendorId = customer.VendorId, OrderId = customer.OrderId, Amount = customer.Amount };

            if (!customer.TransactionType.ToUpper().Equals(PaymentServiceConstants.PaymentType.Auth.ToString().ToUpper()))
                throw new Exception("Server can not find auth transaction");//TODO: треба ще ексепшин хендлінг

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