 using System;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using Newtonsoft.Json;
using DAL.DBModels;
using Stripe;

namespace BLL.Helpers.Mapping
{
    public class MappingStripeSucceeded<T> : IMappingTransaction where T : Charge
    {
        public TransactionDTO Map(PaymentServiceConstants.PaymentType transactionType, PaymentModel payment, dynamic response, DateTime time)
        {
            return new TransactionDTO()
            {
                Amount = payment.Amount,
                Status = response.Status,
                ExternalId = response.Id,
                Instrument = response.PaymentMethodDetails.Type,
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