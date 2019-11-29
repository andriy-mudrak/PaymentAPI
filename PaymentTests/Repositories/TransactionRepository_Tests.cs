using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using DAL.DBModels;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaymentTests
{
    [TestClass]
    public class TransactionRepository_Tests
    {
        private TransactionDTO _transaction1;
        private TransactionDTO _transaction2;
        private List<TransactionDTO> _transactions;

        [TestInitialize()]
        public void Initializer()
        {
            _transaction1 = new TransactionDTO()
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

            _transaction2 = new TransactionDTO()
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
                _transaction1, _transaction2
            };
        }

        [TestMethod]
        public async Task CreateTransactions_ValidModels_Success()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateTransactions_ValidModels_Success")
                .Options;

            using (var context = new PaymentsDbContext(options))
            {
                var service = new TransactionRepository(context);
                await service.CreateTransactions(_transactions);
                context.SaveChanges();
            }

            //Act
            TransactionDTO actual;
            using (var context = new PaymentsDbContext(options))
            {
                actual = await context.Transactions.Select(x => x).LastOrDefaultAsync();
            }
            var expected = _transaction2;

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.Amount, expected.Amount);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Instrument, expected.Instrument);
            Assert.AreEqual(actual.Metadata, expected.Metadata);
            Assert.AreEqual(actual.OrderId, expected.OrderId);
            Assert.AreEqual(actual.Response, expected.Response);
            Assert.AreEqual(actual.Status, expected.Status);
            Assert.AreEqual(actual.TransactionTime, expected.TransactionTime);
            Assert.AreEqual(actual.TransactionType, expected.TransactionType);
            Assert.AreEqual(actual.VendorId, expected.VendorId);
        }

        [TestMethod]
        public async Task GetTransactions_ValidModels_Success()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTransactions_ValidModels_Success")
                .Options;

            using (var context = new PaymentsDbContext(options))
            {
                await context.Transactions.AddRangeAsync(_transactions);
                await context.SaveChangesAsync();
            }

            //Act
            TransactionDTO actual;
            using (var context = new PaymentsDbContext(options))
            {
                var service = new TransactionRepository(context);
                actual = (await service.GetTransactions(x => x.UserId == _transaction2.UserId)).LastOrDefault();
            }
            var expected = _transaction2;

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.Amount, expected.Amount);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Instrument, expected.Instrument);
            Assert.AreEqual(actual.Metadata, expected.Metadata);
            Assert.AreEqual(actual.OrderId, expected.OrderId);
            Assert.AreEqual(actual.Response, expected.Response);
            Assert.AreEqual(actual.Status, expected.Status);
            Assert.AreEqual(actual.TransactionTime, expected.TransactionTime);
            Assert.AreEqual(actual.TransactionType, expected.TransactionType);
            Assert.AreEqual(actual.VendorId, expected.VendorId);
        }
    }
}
