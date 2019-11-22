using BLL.Helpers;

namespace BLL.Services.Interfaces
{
    public interface IPaymentProvider
    {
        IPaymentExecute GetPaymentOperation(PaymentServiceConstants.PaymentType type);
    }
}