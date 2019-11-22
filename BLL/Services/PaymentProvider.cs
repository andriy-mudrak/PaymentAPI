using System.Collections.Generic;
using BLL.Helpers;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public delegate IPaymentExecute ServiceResolver(PaymentServiceConstants.PaymentType key);
    public class PaymentProvider : IPaymentProvider
    {
        private readonly Dictionary<PaymentServiceConstants.PaymentType, IPaymentExecute> PAYMENT_OPERATIONS;

        public PaymentProvider(ServiceResolver serviceAccessor)
        {
            PAYMENT_OPERATIONS = new Dictionary<PaymentServiceConstants.PaymentType, IPaymentExecute>()
            {
                {PaymentServiceConstants.PaymentType.Auth, serviceAccessor(PaymentServiceConstants.PaymentType.Auth)},
                {PaymentServiceConstants.PaymentType.Charge, serviceAccessor(PaymentServiceConstants.PaymentType.Charge)},
                {PaymentServiceConstants.PaymentType.Capture, serviceAccessor(PaymentServiceConstants.PaymentType.Capture)},
                {PaymentServiceConstants.PaymentType.Refund, serviceAccessor(PaymentServiceConstants.PaymentType.Refund)}
            };
        }

        public IPaymentExecute GetPaymentOperation(PaymentServiceConstants.PaymentType type)
        {
            return PAYMENT_OPERATIONS[type];
        }
    }
}