using System.Threading.Tasks;
using BLL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services.Interfaces
{
    public interface IPaymentExecute
    {
        Task Execute(IPaymentRepository _paymentRepository, PaymentModel payment);
    }
}