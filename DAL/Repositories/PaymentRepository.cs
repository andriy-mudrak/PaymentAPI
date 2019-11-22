using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IQueryable<TransactionDTO>> GetTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await TransactionsSelector(orderId, userId, vendorId, startDate, endDate);
        }

        public async Task<TransactionDTO> GetLastTransaction(int orderId)
        {
            return await _context.Transactions.Select(a => a).Where(x => x.OrderId == orderId).LastOrDefaultAsync();
        }
        public async Task<TransactionDTO> GetLastTransaction(int orderId, string transactionType)
        {
            return await _context.Transactions.Select(a => a).Where(x => x.TransactionType == transactionType && x.OrderId == orderId).LastOrDefaultAsync();
        }
        public async Task<UserDTO> CreateUser(UserDTO user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDTO> GetUser(int userId)
        {
            return await (from user in _context.Users
                          where user.UserId == userId
                          select user).FirstOrDefaultAsync();
        }
        public async Task<UserDTO> GetUserByExternalId(string externalId)
        {
            return await (from user in _context.Users
                          where user.ExternalId == externalId
                          select user).FirstOrDefaultAsync();
        }
        public async Task<UserDTO> GetUserByEmail(string email)
        {
            return await (from user in _context.Users
                          where user.Email == email
                          select user).FirstOrDefaultAsync();
        }
        private async Task<IQueryable<TransactionDTO>> TransactionsSelector(int orderId, int userId, int vendorId, DateTime? startDate, DateTime? endDate)
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

