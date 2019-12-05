using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Tests.Constants;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace BLL.Tests
{
    [TestClass]
    public class PaymentProvider_Tests
    {
        private PaymentExecuteBase ServicePaymentResolver(PaymentServiceConstants.PaymentType key)
        {
            switch (key)
            {
                case PaymentServiceConstants.PaymentType.Auth:
                    return new PaymentAuthentication(
                        It.IsAny<ITransactionRepository>(),
                        It.IsAny<IMappingProvider>(),
                        It.IsAny<IRetryHelper>(),
                        It.IsAny<IUserModifier>());
                case PaymentServiceConstants.PaymentType.Capture:
                    return new PaymentCapture(
                        It.IsAny<ITransactionRepository>(),
                        It.IsAny<IMappingProvider>(),
                        It.IsAny<IRetryHelper>(),
                        It.IsAny<ITransactionQueryCreator>());
                case PaymentServiceConstants.PaymentType.Charge:
                    return new PaymentCharge(
                        It.IsAny<ITransactionRepository>(),
                        It.IsAny<IMappingProvider>(),
                        It.IsAny<IRetryHelper>(),
                        It.IsAny<IUserModifier>());
                case PaymentServiceConstants.PaymentType.Refund:
                    return new PaymentRefund(
                        It.IsAny<ITransactionRepository>(),
                        It.IsAny<IMappingProvider>(),
                        It.IsAny<IRetryHelper>(),
                        It.IsAny<ITransactionQueryCreator>());
                default:
                    throw new KeyNotFoundException();
            }
        }

        [TestMethod]
        public void GetPaymentOperation_Auth_Success()
        {
            //Arrange
            ServiceResolver serviceResolver = ServicePaymentResolver;
            var paymentProvider = new PaymentProvider(serviceResolver);

            //Act
            var actual = paymentProvider.GetPaymentOperation(PaymentServiceConstants.PaymentType.Auth);
            var expected = ServicePaymentResolver(PaymentServiceConstants.PaymentType.Auth);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }

        [TestMethod]
        public void GetPaymentOperation_Refund_Success()
        {
            //Arrange
            ServiceResolver serviceResolver = ServicePaymentResolver;
            var paymentProvider = new PaymentProvider(serviceResolver);

            //Act
            var actual = paymentProvider.GetPaymentOperation(PaymentServiceConstants.PaymentType.Refund);
            var expected = ServicePaymentResolver(PaymentServiceConstants.PaymentType.Refund);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }

        [TestMethod]
        public void GetPaymentOperation_Charge_Success()
        {
            //Arrange
            ServiceResolver serviceResolver = ServicePaymentResolver;
            var paymentProvider = new PaymentProvider(serviceResolver);

            //Act
            var actual = paymentProvider.GetPaymentOperation(PaymentServiceConstants.PaymentType.Charge);
            var expected = ServicePaymentResolver(PaymentServiceConstants.PaymentType.Charge);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }

        [TestMethod]
        public void GetPaymentOperation_Capture_Success()
        {
            //Arrange
            ServiceResolver serviceResolver = ServicePaymentResolver;
            var paymentProvider = new PaymentProvider(serviceResolver);

            //Act
            var actual = paymentProvider.GetPaymentOperation(PaymentServiceConstants.PaymentType.Capture);
            var expected = ServicePaymentResolver(PaymentServiceConstants.PaymentType.Capture);

            //Assert
            Assert.IsInstanceOfType(actual, expected.GetType());
        }
    }
}