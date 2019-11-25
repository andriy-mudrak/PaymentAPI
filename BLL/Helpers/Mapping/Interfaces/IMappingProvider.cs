namespace BLL.Helpers.Mapping.Interfaces
{
    public interface IMappingProvider
    {
        IMappingTransaction GetMappingOperation(PaymentServiceConstants.PaymentMappingType mappingType);
    }
}