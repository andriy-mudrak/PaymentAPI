using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentsDbContext _context;

        public TransactionRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionDTO>> CreateTransactions(IEnumerable<TransactionDTO> transaction)
        {
            await _context.Transactions.AddRangeAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactions(Expression<Func<TransactionDTO, bool>> predicate)
        {
            return await _context.Transactions.Where(predicate).ToListAsync();
        }
    }
}

