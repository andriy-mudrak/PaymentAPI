using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stripe;

namespace BLL.Tests.HelpersTests.MappingTests
{
    [TestClass]
    public class MappingProvider_Tests
    {
        private IMappingTransaction MappingPaymentResolver(PaymentServiceConstants.PaymentMappingType key)
        {
            switch (key)
            {
                case PaymentServiceConstants.PaymentMappingType.StripeSucceeded:
                    return new MappingStripeSucceeded<Charge>();
                case PaymentServiceConstants.PaymentMappingType.StripeRefund:
                    return new MappingStripeRefund<Refund>();
                case PaymentServiceConstants.PaymentMappingType.Failed:
                    return new MappingPaymentFailed<string>();
                default:
                    throw new KeyNotFoundException();
            }
        }

        [TestMethod]
        public async Task GetPaymentOperation_StripeSucceeded_Success()
        {
            //Arrange
            MappingResolver mappingResolver = MappingPaymentResolver;
            var paymentProvider = new MappingProvider(mappingResolver);

            //Act
            var actual = paymentProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.StripeSucceeded);
            var expected = MappingPaymentResolver(PaymentServiceConstants.PaymentMappingType.StripeSucceeded);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }

        [TestMethod]
        public async Task GetPaymentOperation_StripeRefund_Success()
        {
            //Arrange
            MappingResolver mappingResolver = MappingPaymentResolver;
            var paymentProvider = new MappingProvider(mappingResolver);

            //Act
            var actual = paymentProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.StripeRefund);
            var expected = MappingPaymentResolver(PaymentServiceConstants.PaymentMappingType.StripeRefund);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }

        [TestMethod]
        public void GetPaymentOperation_Failed_Success()
        {
            //Arrange
            MappingResolver mappingResolver = MappingPaymentResolver;
            var paymentProvider = new MappingProvider(mappingResolver);

            //Act
            var actual = paymentProvider.GetMappingOperation(PaymentServiceConstants.PaymentMappingType.Failed);
            var expected = MappingPaymentResolver(PaymentServiceConstants.PaymentMappingType.Failed);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }
    }
}