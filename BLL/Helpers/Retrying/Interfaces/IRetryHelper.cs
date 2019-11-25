using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Helpers.Interfaces
{
    public interface IRetryHelper
    {
        Task<IEnumerable<TransactionDTO>> RetryIfThrown(Func<Task<TransactionDTO>> action,
            PaymentServiceConstants.PaymentType type,
            PaymentModel payment, PaymentServiceConstants.isSucceeded isSucceeded,
            int triesNumber = RetryConstants.NUMBER_OF_TRIES);
    }
}