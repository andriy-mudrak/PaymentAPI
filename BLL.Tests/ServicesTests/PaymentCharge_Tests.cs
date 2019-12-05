using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
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

namespace BLL.Tests
{
    [TestClass]
    public class PaymentCharge_Tests
    {
        Mock<ITransactionRepository> transactionRepositoryMock;
        Mock<IMappingProvider> mappingProviderMock;
        Mock<IRetryHelper> retryHelperMock;
        Mock<IUserModifier> userModifierMock;
        Mock<PaymentCharge> paymentService;

        [TestInitialize()]
        public void Initializer()
        {
            transactionRepositoryMock = new Mock<ITransactionRepository>();
            mappingProviderMock = new Mock<IMappingProvider>();
            retryHelperMock = new Mock<IRetryHelper>();
            userModifierMock = new Mock<IUserModifier>();

            userModifierMock.Setup(x => x.GetOrCreateUser(It.IsAny<PaymentModel>())).ReturnsAsync(TestConstants.user);

            paymentService = new Mock<PaymentCharge>(transactionRepositoryMock.Object, mappingProviderMock.Object,
                retryHelperMock.Object, userModifierMock.Object);
            paymentService.CallBase = true;

            paymentService.Protected()
                .Setup<Task<IEnumerable<TransactionDTO>>>("ExecuteAndSave", ItExpr.IsAny<Func<Task<TransactionDTO>>>(),
                    ItExpr.IsAny<PaymentServiceConstants.PaymentType>(), ItExpr.IsAny<PaymentModel>(),
                    ItExpr.IsAny<PaymentServiceConstants.IsSucceeded>()).ReturnsAsync(TestConstants.transactionsDTO);
        }

        [TestMethod]
        public async Task Execute_ValidModels_Success()
        {
            //Act
            var actual = await paymentService.Object.Execute(TestConstants.payment);
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