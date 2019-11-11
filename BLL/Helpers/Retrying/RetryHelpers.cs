using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Helpers
{
    public static class RetryHelpers
    {
        public static IEnumerable<TransactionDTO> RetryIfThrown(Func<Task<TransactionDTO>> action, string type, PaymentModel payment, string isSucceeded)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            var timeDelay = RetryConstants.DELAY;

            for (var currentTry = 0; currentTry < RetryConstants.NUMBER_OF_TRIES; currentTry++)
            {
                var transaction = CallPayment(() => action().Result, type, payment);
                transactions.Add(transaction);

                if (transaction.Status == isSucceeded) break;
            }
            return transactions;
        }

        private static TransactionDTO CallPayment(Func<TransactionDTO> action, string type, PaymentModel payment)
        {
            DateTime time = new DateTime();
            try
            {
                time = DateTime.UtcNow;
                return action();

            }
            catch (Exception e)
            {
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.PAYMENT_FAILED].Map(type, payment, e.Message, time);
            }
        }
    }
}