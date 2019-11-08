using System.Collections.Generic;
using BLL.Helpers.Mapping;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using Stripe;

namespace BLL.Helpers
{
    public static class PaymentServiceConstants
    {
        public const string AUTH = "auth";
        public const string CHARGE = "charge";
        public const string CAPTURE = "capture";
        public const string REFUND = "refund";

        public const string ORDER = "order";
        public const string VENDOR = "vendor";
        public const string USER = "user";

        public const string STRIPE_SUCCEEDED = "succeeded";
        public const string PAYMENT_FAILED = "payment failed";

        public enum PaymentType
        {
            Auth,
            Charge,
            Capture,
            Default
        }

        public static readonly Dictionary<string, IMappingTransaction> MAPPING = new Dictionary<string, IMappingTransaction>()
        {
            {STRIPE_SUCCEEDED, new MappingStripeSucceeded<Charge>()},
            {PAYMENT_FAILED, new MappingPaymentFailed<string>()},
        };
    }
}
