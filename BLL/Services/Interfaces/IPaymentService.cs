using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task Pay(string type, PaymentModel request);
        Task<IEnumerable<TransactionModel>> GetTransactions(string type = null, int id = 0, DateTime? startDate = null, DateTime? endDate = null);
    }
}