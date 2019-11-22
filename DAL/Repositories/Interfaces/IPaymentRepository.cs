using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DBModels;

namespace DAL.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IQueryable<TransactionDTO>> GetTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null);
        Task<TransactionDTO> GetLastTransaction(int orderId);
        Task<TransactionDTO> GetLastTransaction(int orderId, string transactionType);
        Task<UserDTO> CreateUser(UserDTO user);
        Task<UserDTO> UpdateUser(UserDTO user);
        Task<UserDTO> GetUserByExternalId(string externalId);
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO> GetUser(int userId);
        Task<IEnumerable<TransactionDTO>> CreateTransactions(IEnumerable<TransactionDTO> transaction);
    }
}