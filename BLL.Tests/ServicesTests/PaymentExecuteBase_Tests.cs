using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Tests.Constants;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace BLL.Tests
{
    [TestClass]
    public class PaymentExecuteBase_Tests
    {
        Mock<ITransactionRepository> transactionRepositoryMock;
        Mock<IMappingProvider> mappingProviderMock;
        Mock<IRetryHelper> retryHelperMock;

        Mock<PaymentExecuteBaseTesting> paymentService;


        public class PaymentExecuteBaseTesting: PaymentExecuteBase
        {
            public PaymentExecuteBaseTesting(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper) : base(paymentRepository, mappingProvider, retryHelper)
            {
            }

            public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<TransactionDTO>> ExecuteAndSaveTesting(Func<Task<TransactionDTO>> action, PaymentServiceConstants.PaymentType type,
                PaymentModel payment, PaymentServiceConstants.IsSucceeded succeeded)
            {
                return base.ExecuteAndSave(action, type, payment, succeeded);
            }
        }

        [TestInitialize()]
        public void Initializer()
        {
            transactionRepositoryMock = new Mock<ITransactionRepository>();
            mappingProviderMock = new Mock<IMappingProvider>();
            retryHelperMock = new Mock<IRetryHelper>();

            retryHelperMock.Setup(x => x.RetryIfThrown(It.IsAny<Func<Task<TransactionDTO>>>(),
                    It.IsAny<PaymentServiceConstants.PaymentType>(),
                    It.IsAny<PaymentModel>(), It.IsAny<PaymentServiceConstants.IsSucceeded>(), It.IsAny<int>()))
                .ReturnsAsync(TestConstants.transactionsDTO);

            transactionRepositoryMock.Setup(x => x.CreateTransactions(It.IsAny<IEnumerable<TransactionDTO>>()))
                .ReturnsAsync(TestConstants.transactionsDTO);

            paymentService = new Mock<PaymentExecuteBaseTesting>(transactionRepositoryMock.Object,
            mappingProviderMock.Object,
            retryHelperMock.Object);
        }

        [TestMethod]
        public async Task ExecuteAndSave_ValidModels_Success()
        {
            //Act
            var actual = await paymentService.Object.ExecuteAndSaveTesting(It.IsAny<Func<Task<TransactionDTO>>>(),
                It.IsAny<PaymentServiceConstants.PaymentType>(),
                It.IsAny<PaymentModel>(), It.IsAny<PaymentServiceConstants.IsSucceeded>());
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