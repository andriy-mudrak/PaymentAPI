using System.Collections.Generic;
using BLL.Helpers;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public delegate PaymentExecuteBase ServiceResolver(PaymentServiceConstants.PaymentType key);
    public class PaymentProvider : IPaymentProvider
    {
        private readonly Dictionary<PaymentServiceConstants.PaymentType, PaymentExecuteBase> _paymentOperations;

        public PaymentProvider(ServiceResolver serviceAccessor)
        {
            _paymentOperations = new Dictionary<PaymentServiceConstants.PaymentType, PaymentExecuteBase>()
            {
                {PaymentServiceConstants.PaymentType.Auth, serviceAccessor(PaymentServiceConstants.PaymentType.Auth)},
                {PaymentServiceConstants.PaymentType.Charge, serviceAccessor(PaymentServiceConstants.PaymentType.Charge)},
                {PaymentServiceConstants.PaymentType.Capture, serviceAccessor(PaymentServiceConstants.PaymentType.Capture)},
                {PaymentServiceConstants.PaymentType.Refund, serviceAccessor(PaymentServiceConstants.PaymentType.Refund)}
            };
        }

        public PaymentExecuteBase GetPaymentOperation(PaymentServiceConstants.PaymentType type)
        {
            return _paymentOperations[type];
        }
    }
}