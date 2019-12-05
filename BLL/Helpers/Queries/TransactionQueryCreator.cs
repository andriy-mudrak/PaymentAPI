using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers.Queries.Interfaces;
using DAL.DBModels;

namespace BLL.Helpers.Queries
{
    public class TransactionQueryCreator : ITransactionQueryCreator
    {
        public async Task<Expression<Func<TransactionDTO, bool>>> GetAllTransactions(int orderId = 0, int userId = 0,
            int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return dto => (orderId.IsZero() || dto.OrderId == orderId)
                          && (userId.IsZero() || dto.UserId == userId)
                          && (vendorId.IsZero() || dto.VendorId == vendorId)
                          && (endDate == null || dto.TransactionTime <= endDate)
                          && (startDate == null || dto.TransactionTime >= startDate);
        }


        public async Task<Expression<Func<TransactionDTO, bool>>> GetTransactionForRefund(int orderId)
        {
            return await GetAllTransactions(orderId);
        }

        public async Task<Expression<Func<TransactionDTO, bool>>> GetTransactionForCapture(int orderId, PaymentServiceConstants.PaymentType type = PaymentServiceConstants.PaymentType.Auth)
        {
            return dto => (orderId.IsZero() || dto.OrderId == orderId)
                          && (!type.Equals(PaymentServiceConstants.PaymentType.Auth) || dto.TransactionType == type.ToString());
        }
    }
}