using System;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using DAL.DBModels;
using Newtonsoft.Json;

namespace BLL.Helpers.Mapping
{
    public class MappingPaymentFailed<T> : IMappingTransaction 
    {
        public TransactionDTO Map(string transactionType, PaymentModel payment, dynamic response, DateTime time)
        {
            return new TransactionDTO()
            {
                Amount = payment.Amount,
                Status = PaymentServiceConstants.PAYMENT_FAILED,
                ExternalId = PaymentServiceConstants.PAYMENT_FAILED,
                Instrument = PaymentServiceConstants.PAYMENT_FAILED,
                OrderId = payment.OrderId,
                UserId = payment.UserId,
                VendorId = payment.VendorId,
                Metadata = JsonConvert.SerializeObject(payment),
                Response = response.ToString(),
                TransactionTime = time,
                TransactionType = transactionType,
            };
        }
    }
}