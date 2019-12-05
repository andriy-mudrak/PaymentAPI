using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DBModels;

namespace BLL.Helpers.Queries.Interfaces
{
    public interface ITransactionQueryCreator
    {
        Task<Expression<Func<TransactionDTO, bool>>> GetAllTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null);
        Task<Expression<Func<TransactionDTO, bool>>> GetTransactionForRefund(int orderId);
        Task<Expression<Func<TransactionDTO, bool>>> GetTransactionForCapture(int orderId, PaymentServiceConstants.PaymentType type);
    }
}