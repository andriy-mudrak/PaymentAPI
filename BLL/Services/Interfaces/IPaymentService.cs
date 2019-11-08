using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<TransactionModel>> Pay(PaymentServiceConstants.PaymentType type, PaymentModel payment);
        Task<IEnumerable<TransactionModel>> GetTransactions(int orderId,  int userId,  int vendorId, DateTime? startDate, DateTime? endDate);
    }
}