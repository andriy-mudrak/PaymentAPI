using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using DAL.Constants;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentsDbContext _context;

        public PaymentRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionDTO>> CreateTransactions(IEnumerable<TransactionDTO> transaction)
        {
            await _context.Transactions.AddRangeAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            var transactions = await TransactionSelector(orderId, userId, vendorId, startDate, endDate);
            return await transactions.ToListAsync();
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

        private async Task<IQueryable<TransactionDTO>> TransactionSelector(int orderId, int userId, int vendorId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions.Select(a => a);
            if (orderId != 0) query = query.Where(a => (a.OrderId == orderId));
            if (userId != 0) query = query.Where(a => (a.UserId == userId));
            if (vendorId != 0) query = query.Where(a => (a.VendorId == vendorId));
            if (startDate != null) query = query.Where(a => startDate < a.TransactionTime);
            if (endDate != null) query = query.Where(a => endDate < a.TransactionTime);

            return query;
        }
    }
}

