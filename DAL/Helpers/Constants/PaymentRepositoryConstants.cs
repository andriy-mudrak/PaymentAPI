using System;
using System.Collections.Generic;
using System.Linq;
using PaymentAPI.DBModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Constants
{
    public static class PaymentRepositoryConstants
    {
        public enum TransactionSelectorType
        {
            OrderId,
            VendorId,
            UserId,
            Default
        }

        public static Func<PaymentsDbContext, TransactionSelectorType, int, DateTime?, DateTime?, IEnumerable<TransactionDTO>> userQuery = EF.CompileQuery(
            (PaymentsDbContext db, TransactionSelectorType type, int id, DateTime? startDate, DateTime? endDate)
                => db.Transactions.Where(a => (a.UserId == id) && (startDate < a.TransactionTime && a.TransactionTime < endDate)).Select(a => a)
        );

        public static Func<PaymentsDbContext, TransactionSelectorType, int, DateTime?, DateTime?, IEnumerable<TransactionDTO>> vendorQuery = EF.CompileQuery(
            (PaymentsDbContext db, TransactionSelectorType type, int id, DateTime? startDate, DateTime? endDate)
                => db.Transactions.Where(a => (a.VendorId == id) && (startDate < a.TransactionTime && a.TransactionTime < endDate)).Select(a => a)
        );

        public static Func<PaymentsDbContext, TransactionSelectorType, int, DateTime?, DateTime?, IEnumerable<TransactionDTO>> orderQuery = EF.CompileQuery(
            (PaymentsDbContext db, TransactionSelectorType type, int id, DateTime? startDate, DateTime? endDate)
            => db.Transactions.Where(a => (a.OrderId == id) && (startDate < a.TransactionTime && a.TransactionTime < endDate)).Select(a => a)
        );

        public static Func<PaymentsDbContext, TransactionSelectorType, int, DateTime?, DateTime?, IEnumerable<TransactionDTO>> defaultQuery = EF.CompileQuery(
            (PaymentsDbContext db, TransactionSelectorType type, int id, DateTime? startDate, DateTime? endDate)
                => db.Transactions.Where(a => startDate < a.TransactionTime && a.TransactionTime < endDate).Select(a => a)
        );
    }
}