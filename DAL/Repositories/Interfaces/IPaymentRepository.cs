using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Constants;
using PaymentAPI.DBModels;

namespace DAL.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task CreateTransaction(TransactionDTO transaction);
        Task<IEnumerable<TransactionDTO>> GetTransactions(PaymentRepositoryConstants.TransactionSelectorType type, int id, DateTime? startDate = null, DateTime? endDate = null);
        void CreateUser(UserDTO user);
        void UpdateUser(UserDTO user);
        UserDTO GetUserByExternalId(string externalId);
        UserDTO GetUser(int userId);
        Task CreateTransaction(IEnumerable<TransactionDTO> transaction);
    }
}