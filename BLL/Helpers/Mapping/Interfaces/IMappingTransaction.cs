using BLL.Models;
using PaymentAPI.DBModels;

namespace BLL.Helpers.Mapping.Interfaces
{
    public interface IMappingTransaction
    {
        TransactionDTO Map(string transactionType, PaymentModel payment, dynamic response);
    }
}