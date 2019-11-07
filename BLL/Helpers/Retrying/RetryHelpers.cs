using System;
using System.Collections.Generic;
using System.Threading;
using BLL.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using PaymentAPI.DBModels;

namespace BLL.Helpers
{
    public static class RetryHelpers
    {
        public static IEnumerable<TransactionDTO> RetryIfThrown(Func<TransactionDTO> action, string type, PaymentModel payment, string isSucceeded)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            //TimerCallback tm = new TimerCallback(CallPayment(action,type,payment));
            //var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromSeconds(5);

            //var timer = new Timer((e) =>
            //{
               
            //    Console.WriteLine("time test");
            //}, null, startTimeSpan, periodTimeSpan);

            for (var currentTry = 0; currentTry < RetryConstants.NUMBER_OF_TRIES; currentTry++)
            {
                transactions.Add(CallPayment(action, type, payment));
                if (transactions[transactions.Count-1].Status == isSucceeded) break;
                //поставити затримку 
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
               return PaymentServiceConstants.MAPPING[PaymentServiceConstants.PAYMENT_FAILED].Transaction(type, payment, e.Message);
            }
        }
    }
}