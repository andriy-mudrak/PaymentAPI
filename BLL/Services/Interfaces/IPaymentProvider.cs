using BLL.Helpers;

namespace BLL.Services.Interfaces
{
    public interface IPaymentProvider
    {
        PaymentExecuteBase GetPaymentOperation(PaymentServiceConstants.PaymentType type);
    }
}