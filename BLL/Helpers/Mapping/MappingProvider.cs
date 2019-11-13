using System.Collections.Generic;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using Stripe;

namespace BLL.Helpers.Mapping
{
    public delegate IMappingTransaction MappingResolver(PaymentServiceConstants.PaymentMappingType key);

    public class MappingProvider : IMappingProvider
    {
        private readonly Dictionary<PaymentServiceConstants.PaymentMappingType, IMappingTransaction> MAPPING;

        public MappingProvider(MappingResolver serviceAccessor)
        {
            MAPPING = new Dictionary<PaymentServiceConstants.PaymentMappingType, IMappingTransaction>()
            {
                {PaymentServiceConstants.PaymentMappingType.Stripe_Succeeded, serviceAccessor(PaymentServiceConstants.PaymentMappingType.Stripe_Succeeded)},
                {PaymentServiceConstants.PaymentMappingType.Failed, serviceAccessor(PaymentServiceConstants.PaymentMappingType.Failed)},
            };
        }
        public IMappingTransaction GetMappingOperation(PaymentServiceConstants.PaymentMappingType mappingType)
        {
            return MAPPING[mappingType];
        }
    }
}