using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Repositories.Interfaces;
using PaymentAPI.DBModels;

namespace BLL.Services.Interfaces
{
    public interface IPaymentExecute
    {
        Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment);
    }
}