using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using PaymentAPI.DBModels;

namespace BLL.Helpers
{
    public static class RetryHelpers
    {
        public static IEnumerable<TransactionDTO> RetryIfThrown(Func<Task<TransactionDTO>> action, string type, PaymentModel payment, string isSucceeded)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();

            for (var currentTry = 0; currentTry < RetryConstants.NUMBER_OF_TRIES; currentTry++)
            {
                transactions.Add(CallPayment(()=>action().Result, type, payment));
                if (transactions[transactions.Count-1].Status == isSucceeded) break;
                //TODO: поставити затримку 
            }
            return transactions;
        }


        private static TransactionDTO CallPayment(Func<TransactionDTO> action, string type, PaymentModel payment)
        {
            try
            {
               return action();
            }
            catch (Exception e)
            {
               return PaymentServiceConstants.MAPPING[PaymentServiceConstants.PAYMENT_FAILED].Map(type, payment, e.Message);
            }
        }
    }
}