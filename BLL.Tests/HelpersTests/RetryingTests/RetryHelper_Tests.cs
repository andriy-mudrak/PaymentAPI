using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Tests.Constants;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace BLL.Tests.HelpersTests.RetryingTests
{
    [TestClass]
    public class RetryHelper_Tests
    {

        Mock<IMappingProvider> mappingProviderMock;

        Mock<RetryHelper> retryHelper;
        Func<Task<TransactionDTO>> action;

        [TestInitialize()]
        public void Initializer()
        {
            mappingProviderMock = new Mock<IMappingProvider>();

            retryHelper = new Mock<RetryHelper>(mappingProviderMock.Object);

            action = new Func<Task<TransactionDTO>>(
                () =>
                {
                    return Task.FromResult<TransactionDTO>(TestConstants.transactionsDTO.LastOrDefault());
                });
        }

        [TestMethod]
        public async Task RetryIfThrown_ValidModels_Success()
        {
            //Act
            var actual = await retryHelper.Object.RetryIfThrown(
                action,
                It.IsAny<PaymentServiceConstants.PaymentType>(),
                It.IsAny<PaymentModel>(),
                It.IsAny<PaymentServiceConstants.IsSucceeded>(),
                It.IsAny<int>());

            var expected = TestConstants.transactionsDTO.LastOrDefault();

            var actualLast = actual.LastOrDefault();
            //Assert
            Assert.AreEqual(expected.UserId, actualLast.UserId);
            Assert.AreEqual(expected.Amount, actualLast.Amount);
            Assert.AreEqual(expected.ExternalId, actualLast.ExternalId);
            Assert.AreEqual(expected.Instrument, actualLast.Instrument);
            Assert.AreEqual(expected.Metadata, actualLast.Metadata);
            Assert.AreEqual(expected.OrderId, actualLast.OrderId);
            Assert.AreEqual(expected.Response, actualLast.Response);
            Assert.AreEqual(expected.Status, actualLast.Status);
            Assert.AreEqual(expected.TransactionTime, actualLast.TransactionTime);
            Assert.AreEqual(expected.TransactionType, actualLast.TransactionType);
            Assert.AreEqual(expected.VendorId, actualLast.VendorId);
        }
    }
}