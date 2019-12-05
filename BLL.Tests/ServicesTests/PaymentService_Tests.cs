using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
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
    public class PaymentService_Tests
    {
        private Mock<ITransactionRepository> _paymentRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPaymentProvider> _paymentProviderMock;
        private Mock<ITransactionQueryCreator> _queryCreatorMock;

        private Mock<PaymentService> paymentService;

        public class PaymentExecuteBaseTesting : PaymentExecuteBase
        {
            public PaymentExecuteBaseTesting(ITransactionRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
                : base(paymentRepository, mappingProvider, retryHelper)
            {
            }

            public override async Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment)
            {
                return new List<TransactionDTO>();
            }
        }


        [TestInitialize()]
        public void Initializer()
        {
            _paymentRepositoryMock = new Mock<ITransactionRepository>();
            _mapperMock = new Mock<IMapper>();
            _paymentProviderMock = new Mock<IPaymentProvider>();
            _queryCreatorMock = new Mock<ITransactionQueryCreator>();

            _paymentProviderMock.Setup(x => x.GetPaymentOperation(It.IsAny<PaymentServiceConstants.PaymentType>()))
                .Returns(new PaymentExecuteBaseTesting(
                    It.IsAny<ITransactionRepository>(),
                    It.IsAny<IMappingProvider>(),
                    It.IsAny<IRetryHelper>()));

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(It.IsAny<IEnumerable<TransactionDTO>>()))
                .Returns(TestConstants.transactions);

            _queryCreatorMock.Setup(x => x.GetAllTransactions(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()));

            _paymentRepositoryMock.Setup(
                x => x.GetTransactions(It.IsAny <Expression<Func<TransactionDTO, bool>>>()));

            paymentService = new Mock<PaymentService>(
                _paymentRepositoryMock.Object,
                _mapperMock.Object,
                _paymentProviderMock.Object,
                _queryCreatorMock.Object);
        }

        [TestMethod]
        public async Task Pay_ValidModels_Success()
        {
            //Act
            var actual = await paymentService.Object.Pay(It.IsAny<PaymentServiceConstants.PaymentType>(), It.IsAny<PaymentModel>());
            var expected = TestConstants.transactions.LastOrDefault();

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

        [TestMethod]
        public async Task GetTransactions_ValidModels_Success()
        {
            //Act
            var actual = await paymentService.Object.GetTransactions(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>());

            var expected = TestConstants.transactions.LastOrDefault();

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