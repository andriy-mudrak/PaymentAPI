using System;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Helpers.Mapping.Interfaces
{
    public interface IMappingTransaction
    {
        TransactionDTO Map(string transactionType, PaymentModel payment, dynamic response, DateTime time);
    }
}