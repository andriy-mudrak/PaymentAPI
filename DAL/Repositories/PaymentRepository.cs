using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using DAL.Constants;
using DAL.Repositories.Interfaces;
using PaymentAPI.DBModels;

namespace DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentsDbContext _context;

        public PaymentRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task CreateTransaction(TransactionDTO transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            _context.SaveChanges();
        }

        public async Task CreateTransaction(IEnumerable<TransactionDTO> transaction)
        {
            await _context.Transactions.AddRangeAsync(transaction);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactions(PaymentRepositoryConstants.TransactionSelectorType type, int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            var beginingDate = startDate == null ? SqlDateTime.MinValue.Value : startDate;
            var endingDate = endDate == null ? SqlDateTime.MaxValue.Value : endDate;

            var transactions = await TransactionSelector(type, id, beginingDate, endingDate);

            return transactions;
        }

        public void CreateUser(UserDTO user)
        {

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(UserDTO user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public UserDTO GetUser(int userId)
        {
            return (from user in _context.Users
                    where user.UserId == userId
                    select user).FirstOrDefault();
        }
        public UserDTO GetUserByExternalId(string externalId)
        {
            return (from user in _context.Users
                    where user.ExternalId == externalId
                    select user).FirstOrDefault();
        }

        private async Task<IEnumerable<TransactionDTO>> TransactionSelector(PaymentRepositoryConstants.TransactionSelectorType type, int id, DateTime? startDate, DateTime? endDate)
        {
            switch (type)
            {
                case PaymentRepositoryConstants.TransactionSelectorType.OrderId:
                    return PaymentRepositoryConstants.orderQuery(_context, type, id, startDate, endDate);
                case PaymentRepositoryConstants.TransactionSelectorType.VendorId:
                    return PaymentRepositoryConstants.vendorQuery(_context, type, id, startDate, endDate);
                case PaymentRepositoryConstants.TransactionSelectorType.UserId:
                    return PaymentRepositoryConstants.userQuery(_context, type, id, startDate, endDate);
                default:
                    return PaymentRepositoryConstants.defaultQuery(_context, type, id, startDate, endDate);
            }
        }
    }
}
