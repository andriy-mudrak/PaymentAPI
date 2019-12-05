using BLL.Helpers.Validators;
using BLL.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace BLL.Helpers
{
    public static class RequestTypeValidator
    {
        public static bool TypeValidation(PaymentModel payment)
        {
            switch (payment.Type)
            {
                case PaymentServiceConstants.CHARGE:
                case PaymentServiceConstants.AUTH:
                    {
                        var validator = new PaymentCreatingValidator();
                        var test = validator.Validate(payment);
                        return test.IsValid;
                    }

                case PaymentServiceConstants.CAPTURE:
                case PaymentServiceConstants.REFUND:
                    {
                        var validator = new PaymentUpdatingValidator();
                        var test = validator.Validate(payment);
                        return test.IsValid;
                    }

                default: return false;
            }
        }

        public static PaymentServiceConstants.PaymentType PaymentChecker(string type)
        {
            switch (type)
            {
                case PaymentServiceConstants.AUTH: return PaymentServiceConstants.PaymentType.Auth;
                case PaymentServiceConstants.CHARGE: return PaymentServiceConstants.PaymentType.Charge;
                case PaymentServiceConstants.CAPTURE: return PaymentServiceConstants.PaymentType.Capture;
                case PaymentServiceConstants.REFUND: return PaymentServiceConstants.PaymentType.Refund;
                default: return PaymentServiceConstants.PaymentType.Default;
            }
        }
    }
}