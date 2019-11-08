using System;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using Newtonsoft.Json;
using PaymentAPI.DBModels;

namespace BLL.Helpers.Mapping
{
    public class MappingPaymentFailed<T> : IMappingTransaction 
    {
        public TransactionDTO Map(string transactionType, PaymentModel payment, dynamic response)
        {
            return new TransactionDTO()
            {
                Amount = 0,
                Status = PaymentServiceConstants.PAYMENT_FAILED,
                ExternalId = PaymentServiceConstants.PAYMENT_FAILED,
                Instrument = PaymentServiceConstants.PAYMENT_FAILED,
                OrderId = payment.OrderId,
                UserId = payment.UserId,
                VendorId = payment.VendorId,
                Metadata = JsonConvert.SerializeObject(payment),
                Response = response.ToString(),
                TransactionTime = DateTime.Now,
                TransactionType = transactionType,
                Description = "didn`t find it in response"
            };
        }
    }
}