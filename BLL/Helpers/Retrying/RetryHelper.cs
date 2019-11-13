using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using DAL.DBModels;
using Polly;

namespace BLL.Helpers
{
    public class RetryHelper:IRetryHelper
    {
        private readonly IMappingProvider _mappingProvider;
        public RetryHelper(IMappingProvider mappingProvider)
        {
            _mappingProvider = mappingProvider;
        }
        public async Task<IEnumerable<TransactionDTO>> RetryIfThrown(Func<Task<TransactionDTO>> action, PaymentServiceConstants.PaymentType type, 
            PaymentModel payment, PaymentServiceConstants.isSucceeded isSucceeded, int triesNumber = RetryConstants.NUMBER_OF_TRIES)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>(3);
            var timeDelay = RetryConstants.DELAY;
            
            var retryPolicy = Policy
                .HandleResult<IEnumerable<TransactionDTO>>(x => !x.LastOrDefault().Status.ToUpper().Equals(isSucceeded.ToString().ToUpper()))
                .WaitAndRetryAsync(triesNumber,
                    i =>
                    {
                        var time = TimeSpan.FromSeconds(timeDelay);
                        timeDelay = timeDelay * RetryConstants.EXPONENT_TIME_PARAMETER;
                        return time;
                    });
            
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var transaction = await CallPayment(() => action(), type, payment);

                transactions.Add(transaction);
                return transactions;
            }); 
        }

        private async Task<TransactionDTO> CallPayment(Func<Task<TransactionDTO>> action, PaymentServiceConstants.PaymentType type, PaymentModel payment)
        {
            DateTime time = DateTime.UtcNow;

            try
            {
                return  await action();
            }
            catch (Exception e)
            {
                return _mappingProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.Failed).Map(type, payment,
                    e.Message, time);
            }
        }
    }
}