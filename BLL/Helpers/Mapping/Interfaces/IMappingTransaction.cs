using System;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Helpers.Mapping.Interfaces
{
    public interface IMappingTransaction
    {
        TransactionDTO Map(PaymentServiceConstants.PaymentType transactionType, PaymentModel payment, dynamic response, DateTime time);
    }
}