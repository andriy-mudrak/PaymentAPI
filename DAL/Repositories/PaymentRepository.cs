using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using DAL.Constants;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<TransactionDTO> CreateTransaction(TransactionDTO transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            _context.SaveChanges();

            return transaction;
        }

        public async Task<IEnumerable<TransactionDTO>> CreateTransaction(IEnumerable<TransactionDTO> transaction)
        {
            await _context.Transactions.AddRangeAsync(transaction);
            _context.SaveChanges();
            return transaction;
        }


        //треба передавати сюди експрешин, якщо старт дейт і енддейт то не додавати його в цей експрешин
        // і обов'ящково треба робити 
        public async Task<IEnumerable<TransactionDTO>> GetTransactions(int orderId = 0, int userId = 0, int vendorId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            var expr = _context.Transactions.Where(a => startDate < a.TransactionTime && a.TransactionTime < endDate)
                .Select(a => a);

            var transactions = await TransactionSelector(orderId, userId, vendorId, startDate, endDate);

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

        private async Task<IEnumerable<TransactionDTO>> TransactionSelector(int orderId, int userId, int vendorId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions.Select(a => a);
            if (orderId != 0) query = query.Where(a => (a.OrderId == orderId));
            if (userId != 0) query = query.Where(a => (a.UserId == userId));
            if (vendorId != 0) query = query.Where(a => (a.VendorId == vendorId));
            if (startDate != null) query = query.Where(a => startDate < a.TransactionTime);
            if (endDate != null) query = query.Where(a => endDate < a.TransactionTime);

            return query.ToList();
            //db.Transactions.Where(a => (a.UserId == id) && (startDate < a.TransactionTime && a.TransactionTime < endDate)).Select(a => a)
            //case PaymentRepositoryConstants.TransactionSelectorType.OrderId:
            //        return PaymentRepositoryConstants.orderQuery(_context, type, id, startDate, endDate);
            //    case PaymentRepositoryConstants.TransactionSelectorType.VendorId:
            //        return PaymentRepositoryConstants.vendorQuery(_context, type, id, startDate, endDate);
            //    case PaymentRepositoryConstants.TransactionSelectorType.UserId:
            //        return PaymentRepositoryConstants.userQuery(_context, type, id, startDate, endDate);
            //    default:
            //        return PaymentRepositoryConstants.defaultQuery(_context, type, id, startDate, endDate);
        }
    }
}

