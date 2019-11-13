using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BLL.Models;
using DAL.DBModels;
using Polly;

namespace BLL.Helpers
{
    public static class RetryHelpers
    {
        public static async Task<IEnumerable<TransactionDTO>> RetryIfThrown(Func<Task<TransactionDTO>> action, string type, PaymentModel payment, string isSucceeded)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            var timeDelay = RetryConstants.DELAY;
            var retryPolicy = Policy
                .HandleResult<IEnumerable<TransactionDTO>>(x => !x.LastOrDefault().Status.Equals(PaymentServiceConstants.STRIPE_SUCCEEDED))
                .WaitAndRetryAsync(RetryConstants.NUMBER_OF_TRIES,
                    i =>
                    {
                        var time = TimeSpan.FromSeconds(timeDelay);
                        timeDelay = timeDelay * RetryConstants.EXPONENT_TIME_PARAMETER;
                        return time;
                    });

            var test = await retryPolicy.ExecuteAsync(async () =>
            {
                var transaction = await CallPayment(() => action(), type, payment);
               
                transactions.Add(transaction);
                return transactions;
            });

            return transactions;
        }

        private static async Task<TransactionDTO> CallPayment(Func<Task<TransactionDTO>> action, string type, PaymentModel payment)
        {
            DateTime time = new DateTime();
            try
            {
                time = DateTime.UtcNow;
                return await action();

            }
            catch (Exception e)
            {
                return PaymentServiceConstants.MAPPING[PaymentServiceConstants.PAYMENT_FAILED].Map(type, payment, e.Message, time);
            }
        }
    }
}