using System.Collections.Generic;
using System.ComponentModel;
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
        public const string DEFAULT = "default";

        public const string ORDER = "order";
        public const string VENDOR = "vendor";
        public const string USER = "user";
        
        public enum PaymentType
        {
            Auth,
            Charge,
            Capture,
            Refund,
            Default
        }

        public enum PaymentMappingType
        {
            Stripe_Succeeded,
            Stripe_Refund,
            Failed
        }

        public enum isSucceeded
        {
            Succeeded,
        }
    }
}
