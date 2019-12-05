using System.Collections.Generic;
using BLL.Helpers.Mapping.Interfaces;

namespace BLL.Helpers.Mapping
{
    public delegate IMappingTransaction MappingResolver(PaymentServiceConstants.PaymentMappingType key);

    public class MappingProvider : IMappingProvider
    {
        private readonly Dictionary<PaymentServiceConstants.PaymentMappingType, IMappingTransaction> _mapping;

        public MappingProvider(MappingResolver serviceAccessor)
        {
            _mapping = new Dictionary<PaymentServiceConstants.PaymentMappingType, IMappingTransaction>()
            {
                {PaymentServiceConstants.PaymentMappingType.StripeSucceeded, serviceAccessor(PaymentServiceConstants.PaymentMappingType.StripeSucceeded)},
                {PaymentServiceConstants.PaymentMappingType.StripeRefund, serviceAccessor(PaymentServiceConstants.PaymentMappingType.StripeRefund)},
                {PaymentServiceConstants.PaymentMappingType.Failed, serviceAccessor(PaymentServiceConstants.PaymentMappingType.Failed)},
            };
        }
        public IMappingTransaction GetMappingOperation(PaymentServiceConstants.PaymentMappingType mappingType)
        {
            return _mapping[mappingType];
        }
    }
}