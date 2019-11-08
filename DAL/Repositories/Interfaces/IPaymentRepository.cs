using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Constants;
using PaymentAPI.DBModels;

namespace DAL.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<TransactionDTO> CreateTransaction(TransactionDTO transaction);
        Task<IEnumerable<TransactionDTO>> GetTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate=null, DateTime? endDate=null);
        void CreateUser(UserDTO user);
        void UpdateUser(UserDTO user);
        UserDTO GetUserByExternalId(string externalId);
        UserDTO GetUser(int userId);
        Task<IEnumerable<TransactionDTO>> CreateTransaction(IEnumerable<TransactionDTO> transaction);
    }
}