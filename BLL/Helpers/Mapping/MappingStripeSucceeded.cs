using System;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using Newtonsoft.Json;
using PaymentAPI.DBModels;
using Stripe;

namespace BLL.Helpers.Mapping
{
    public class MappingStripeSucceeded<T> : IMappingTransaction where T : Charge
    {
        public TransactionDTO Transaction(string transactionType, PaymentModel payment, dynamic response)
        {
            return new TransactionDTO()
            {
                Amount = response.Amount,
                Status = response.Status,
                ExternalId = response.Id,
                Instrument = response.PaymentMethodDetails.Type,
                OrderId = payment.OrderId,
                UserId = payment.UserId,
                VendorId = payment.VendorId,
                Metadata = JsonConvert.SerializeObject(payment),
                Response = JsonConvert.SerializeObject(response),
                TransactionTime = DateTime.Now,
                TransactionType = transactionType,
                Description = "didn`t find it in response"
            };
        }

        //public TransactionDTO Transaction(string transactionType, PaymentModel payment, Charge response)
        //{
        //    return new TransactionDTO()
        //    {
        //        Amount = response.Amount,
        //        Status = response.Status,
        //        ExternalId = response.Id,
        //        Instrument = response.PaymentMethodDetails.Type,
        //        OrderId = payment.OrderId,
        //        UserId = payment.UserId,
        //        VendorId = payment.VendorId,
        //        Metadata = JsonConvert.SerializeObject(payment),
        //        Response = JsonConvert.SerializeObject(response),
        //        TransactionTime = DateTime.Now,
        //        TransactionType = transactionType,
        //        Description = "didn`t find it in response"
        //    };
        //}


    }
}