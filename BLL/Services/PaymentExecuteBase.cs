using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using DAL.DBModels;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Interfaces
{
    public abstract class PaymentExecuteBase
    {
        protected readonly ITransactionRepository PaymentRepository;
        protected readonly IMappingProvider MappingProvider;
        protected readonly IRetryHelper RetryHelper;

        protected PaymentExecuteBase(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
        {
            PaymentRepository = paymentRepository;
            MappingProvider = mappingProvider;
            RetryHelper = retryHelper;
        }

        protected virtual async Task<IEnumerable<TransactionDTO>> ExecuteAndSave(Func<Task<TransactionDTO>> action, PaymentServiceConstants.PaymentType type, 
            PaymentModel payment, PaymentServiceConstants.IsSucceeded succeeded)
        {
            var transaction = await RetryHelper.RetryIfThrown(action, type, payment, succeeded);

            return await PaymentRepository.CreateTransactions(transaction);
        }

        public abstract Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment);
    }
}