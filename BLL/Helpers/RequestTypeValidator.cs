using BLL.Models;
using DAL.Constants;
using Microsoft.EntityFrameworkCore.Internal;

namespace BLL.Helpers
{
    public static class RequestTypeValidator
    {
        public static bool TypeValidation(string type, PaymentModel model)
        {
            switch (type)
            {
                case PaymentServiceConstants.CHARGE: return CreationPayment(model);
                case PaymentServiceConstants.AUTH: return CreationPayment(model);
                case PaymentServiceConstants.CAPTURE: return UpdationPayment(model);
                case PaymentServiceConstants.REFUND: return UpdationPayment(model);
                default: return false;
            }
        }

        public static PaymentRepositoryConstants.TransactionSelectorType GetTransactions_TypeChecker(string type)
        {
            switch (type)
            {
                case PaymentServiceConstants.USER: return PaymentRepositoryConstants.TransactionSelectorType.UserId;
                case PaymentServiceConstants.VENDOR: return  PaymentRepositoryConstants.TransactionSelectorType.VendorId;
                case PaymentServiceConstants.ORDER: return PaymentRepositoryConstants.TransactionSelectorType.OrderId;
                default: return PaymentRepositoryConstants.TransactionSelectorType.Default;
            }
        }

        private static bool CreationPayment(PaymentModel charge)
        {
            bool isChargeModel = true;
            isChargeModel= charge.Email.Any();
            isChargeModel = !charge.Amount.IsZero();
            isChargeModel = charge.CardToken.Any();
            isChargeModel = !charge.UserId.IsZero();
            isChargeModel = !charge.OrderId.IsZero();

            return isChargeModel;
        }

        private static bool UpdationPayment(PaymentModel charge)
        {
            bool isChargeModel = true;
            isChargeModel = !charge.OrderId.IsZero();

            return isChargeModel;
        }
    }
}