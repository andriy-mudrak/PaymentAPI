using System;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using DAL.DBModels;
using Newtonsoft.Json;
using Stripe;

namespace BLL.Helpers.Mapping
{
    public class MappingStripeRefund<T> : IMappingTransaction where T : Refund
    {
        public TransactionDTO Map(PaymentServiceConstants.PaymentType transactionType, PaymentModel payment, dynamic response, DateTime time)
        {
            return new TransactionDTO()
            {
                Amount = payment.Amount,
                Status = response.Status,
                ExternalId = response.Id,
                Instrument = null,
                OrderId = payment.OrderId,
                UserId = payment.UserId,
                VendorId = payment.VendorId,
                Metadata = JsonConvert.SerializeObject(payment),
                Response = response.ToJson(),
                TransactionTime = time,
                TransactionType = transactionType.ToString(),
            };
        }
    }
}