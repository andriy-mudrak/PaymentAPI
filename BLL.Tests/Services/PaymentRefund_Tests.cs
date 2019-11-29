using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stripe;

namespace BLL.Tests
{
    [TestClass]
    public class PaymentRefund_Tests
    {
        private Mock<ITransactionRepository> transactionRepositoryMock;
        private Mock<IMappingProvider> mappingProviderMock;
        private Mock<IRetryHelper> retryHelperMock;
        private Mock<ITransactionQueryCreator> queryCreator;
        private PaymentExecuteBase paymentService;

        private List<TransactionDTO> _transactions;
        private PaymentModel _payment;
        [TestInitialize()]
        public void Initializer()
        {
            var user = new UserDTO()
            {
                Email = "test@mail.com",
                ExternalId = "cus_lalalalallalaa",
                UserToken = "tok_okasdlmadsmamasd"
            };

            _payment = new PaymentModel
            {
                Amount = 1000,
                CardToken = "tok_visa",
                Currency = "usd",
                Email = "kram@mail.com",
                OrderId = 1,
                VendorId = 1,
                SaveCard = false,
                Type = "auth",
                UserId = 1,
            };

            var transaction1 = new TransactionDTO()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata",
                TransactionTime = DateTime.Now,
                UserId = 1,
                OrderId = 1,
                VendorId = 1,
                Instrument = "card",
                Response = "some response",
            };

            var transaction2 = new TransactionDTO()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge retry",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata retry",
                TransactionTime = DateTime.Now,
                UserId = 2,
                OrderId = 2,
                VendorId = 3,
                Instrument = "card retry",
                Response = "some response retry",
            };

            _transactions = new List<TransactionDTO>
            {
                transaction1, transaction2
            };

            transactionRepositoryMock = new Mock<ITransactionRepository>();
            mappingProviderMock = new Mock<IMappingProvider>();
            retryHelperMock = new Mock<IRetryHelper>();

            mappingProviderMock.Setup(x =>
                x.GetMappingOperation(It.IsAny<PaymentServiceConstants.PaymentMappingType>())
                    .Map(It.IsAny<PaymentServiceConstants.PaymentType>(), It.IsAny<PaymentModel>(),
                        It.IsAny<Charge>(), It.IsAny<DateTime>())).Returns(It.IsAny<TransactionDTO>());

            transactionRepositoryMock.Setup(x => x.CreateTransactions(It.IsAny<IEnumerable<TransactionDTO>>()))
                .ReturnsAsync(_transactions);

            transactionRepositoryMock.Setup(x => x.GetTransactions(It.IsAny<Expression<Func<TransactionDTO, bool>>>()))
                .ReturnsAsync(It.IsAny<IEnumerable<TransactionDTO>>());

            retryHelperMock.Setup(x => x.RetryIfThrown(It.IsAny<Func<Task<TransactionDTO>>>(),
            It.IsAny<PaymentServiceConstants.PaymentType>(), It.IsAny<PaymentModel>(),
            It.IsAny<PaymentServiceConstants.IsSucceeded>(), It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<IEnumerable<TransactionDTO>>());

            queryCreator.Setup(x => x.GetTransactionForRefund(It.IsAny<int>()))
                .Returns(It.IsAny<Task<Expression<Func<TransactionDTO, bool>>>>());

            paymentService = new Services.PaymentRefund(
                transactionRepositoryMock.Object,
                mappingProviderMock.Object,
                retryHelperMock.Object,
                queryCreator.Object);
        }

        [TestMethod]
        public async Task CreateTransactions_ValidModels_Success()
        {
            var expected = await paymentService.Execute(_payment);
            var actual = _transactions.LastOrDefault();

            var expectedLast = expected.LastOrDefault();
            //Assert
            Assert.AreEqual(actual.UserId, expectedLast.UserId);
            Assert.AreEqual(actual.Amount, expectedLast.Amount);
            Assert.AreEqual(actual.ExternalId, expectedLast.ExternalId);
            Assert.AreEqual(actual.Instrument, expectedLast.Instrument);
            Assert.AreEqual(actual.Metadata, expectedLast.Metadata);
            Assert.AreEqual(actual.OrderId, expectedLast.OrderId);
            Assert.AreEqual(actual.Response, expectedLast.Response);
            Assert.AreEqual(actual.Status, expectedLast.Status);
            Assert.AreEqual(actual.TransactionTime, expectedLast.TransactionTime);
            Assert.AreEqual(actual.TransactionType, expectedLast.TransactionType);
            Assert.AreEqual(actual.VendorId, expectedLast.VendorId);
        }
    }
}