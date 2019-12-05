//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using BLL.Helpers;
//using DAL.DBModels;
//using DAL.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Payment.Tests
//{
//    [TestClass]
//    public class PaymentRepositoryTests
//    {
//        [TestMethod]
//        public async Task GetUserByEmail_ValidUser_Succeeded()
//        {
//            //arrange
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetUserByEmail_ValidUser_Succeeded").Options;

//            var context = new PaymentsDbContext(options);

//            var user = new UserDTO()
//            {
//                Email = "test@mail.com",
//                ExternalId = "ch_externalidtest",
//                UserToken = "tok_userToken"

//            };
//            await context.Users.AddAsync(user);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);
//            var actual = await repository.GetUserByEmail(user.Email);
//            var expected = new UserDTO()
//            {
//                UserId = 1,
//                Email = "test@mail.com",
//                ExternalId = "ch_externalidtest",
//                UserToken = "tok_userToken"
//            };


//            //assert 
//            if (actual == null) Assert.Fail();
//            Assert.IsTrue(actual.Email == expected.Email);
//            Assert.IsTrue(actual.ExternalId == expected.ExternalId);
//            Assert.IsTrue(actual.UserId == expected.UserId);
//            Assert.IsTrue(actual.UserToken == expected.UserToken);
//        }

//        [TestMethod]
//        public async Task GetUser_ValidUser_Succeeded()
//        {
//            //arrange 
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetUser_ValidUser_Succeeded").Options;

//            var context = new PaymentsDbContext(options);
//            var user = new UserDTO()
//            {
//                UserId = 1,
//                Email = "test@mail.com",
//                ExternalId = "ch_externalidtest",
//                UserToken = "tok_userToken"
//            };
//            await context.Users.AddAsync(user);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);
//            var actual = await repository.GetUser(1);
//            var expected = user;

//            //Assert
//            if (actual == null) Assert.Fail();
//            Assert.IsTrue(actual.Email == expected.Email);
//            Assert.IsTrue(actual.ExternalId == expected.ExternalId);
//            Assert.IsTrue(actual.UserId == expected.UserId);
//            Assert.IsTrue(actual.UserToken == expected.UserToken);
//        }

//        [TestMethod]
//        public async Task GetLastTransactionByType_ValidTransaction_Succeeded()
//        {
//            //arrange 
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetLastTransactionByType_ValidTransaction_Succeeded").Options;

//            var context = new PaymentsDbContext(options);
//            var transactionFirst = new TransactionDTO()
//            {
//                TransactionId = 1,
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "not last transaction",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };
//            var transactionLast = new TransactionDTO()
//            {
//                TransactionId = 2,
//                ExternalId = "ch_externalidtest_last",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card last",
//                Metadata = "last transaction",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response last",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };
//            await context.Transactions.AddRangeAsync(transactionFirst, transactionLast);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);

//            var actual = await repository.GetLastTransaction(2, PaymentServiceConstants.PaymentType.Auth.ToString());
//            var expected = transactionLast;

//            //assert
//            Assert.IsTrue(actual.TransactionId == expected.TransactionId);
//            Assert.IsTrue(actual.ExternalId == expected.ExternalId);
//            Assert.IsTrue(actual.UserId == expected.UserId);
//            Assert.IsTrue(actual.Amount == expected.Amount);
//            Assert.IsTrue(actual.Instrument == expected.Instrument);
//            Assert.IsTrue(actual.Metadata == expected.Metadata);
//            Assert.IsTrue(actual.OrderId == expected.OrderId);
//            Assert.IsTrue(actual.VendorId == expected.VendorId);
//            Assert.IsTrue(actual.Response == expected.Response);
//            Assert.IsTrue(actual.Status == expected.Status);
//            Assert.IsTrue(actual.TransactionType == expected.TransactionType);
//            Assert.IsTrue(actual.TransactionTime == expected.TransactionTime);

//        }

//        [TestMethod]
//        public async Task GetLastTransaction_ValidTransaction_Succeeded()
//        {
//            //arrange 
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetLastTransaction_ValidTransaction_Succeeded").Options;

//            var context = new PaymentsDbContext(options);
//            var transactionFirst = new TransactionDTO()
//            {
//                TransactionId = 1,
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "not last transaction",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };
//            var transactionLast = new TransactionDTO()
//            {
//                TransactionId = 2,
//                ExternalId = "ch_externalidtest_last",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card last",
//                Metadata = "last transaction",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response last",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };
//            await context.Transactions.AddRangeAsync(transactionFirst, transactionLast);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);

//            var actual = await repository.GetLastTransaction(2);
//            var expected = transactionLast;

//            //assert
//            Assert.IsTrue(actual.TransactionId == expected.TransactionId);
//            Assert.IsTrue(actual.ExternalId == expected.ExternalId);
//            Assert.IsTrue(actual.UserId == expected.UserId);
//            Assert.IsTrue(actual.Amount == expected.Amount);
//            Assert.IsTrue(actual.Instrument == expected.Instrument);
//            Assert.IsTrue(actual.Metadata == expected.Metadata);
//            Assert.IsTrue(actual.OrderId == expected.OrderId);
//            Assert.IsTrue(actual.VendorId == expected.VendorId);
//            Assert.IsTrue(actual.Response == expected.Response);
//            Assert.IsTrue(actual.Status == expected.Status);
//            Assert.IsTrue(actual.TransactionType == expected.TransactionType);
//            Assert.IsTrue(actual.TransactionTime == expected.TransactionTime);
//        }

//        [TestMethod]
//        public async Task CreateTransactions_ValidTransaction_Succeeded()
//        {
//            //arrange 
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "CreateTransactions_ValidTransaction_Succeeded").Options;

//            var context = new PaymentsDbContext(options);
//            var transactionFirst = new TransactionDTO()
//            {
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "Metadata",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };

//            var transactionSecond = new TransactionDTO()
//            {
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "Metadata",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow

//            };
//            var transactions = new List<TransactionDTO>() { transactionFirst, transactionSecond };

//            //act 
//            var repository = new PaymentRepository(context);
//            await repository.CreateTransactions(transactions);
//            var actual = await context.Transactions.Select(x => x).ToListAsync();

//            var expected = transactions;

//            //assert
//            for (int i = 0; i < transactions.Count; i++)
//            {
//                Assert.IsTrue(actual[0].TransactionId == expected[0].TransactionId);
//                Assert.IsTrue(actual[0].ExternalId == expected[0].ExternalId);
//                Assert.IsTrue(actual[0].UserId == expected[0].UserId);
//                Assert.IsTrue(actual[0].Amount == expected[0].Amount);
//                Assert.IsTrue(actual[0].Instrument == expected[0].Instrument);
//                Assert.IsTrue(actual[0].Metadata == expected[0].Metadata);
//                Assert.IsTrue(actual[0].OrderId == expected[0].OrderId);
//                Assert.IsTrue(actual[0].VendorId == expected[0].VendorId);
//                Assert.IsTrue(actual[0].Response == expected[0].Response);
//                Assert.IsTrue(actual[0].Status == expected[0].Status);
//                Assert.IsTrue(actual[0].TransactionType == expected[0].TransactionType);
//                Assert.IsTrue(actual[0].TransactionTime == expected[0].TransactionTime);
//            }
//        }
//        [TestMethod]
//        public async Task GetTransactions_ValidTransaction_Succeeded()
//        {
//            //arrange 
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetTransactions_ValidTransaction_Succeeded").Options;

//            var context = new PaymentsDbContext(options);
//            var transactionFirst = new TransactionDTO()
//            {
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "Metadata",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow
//            };

//            var transactionSecond = new TransactionDTO()
//            {
//                ExternalId = "ch_externalidtest",
//                UserId = 2,
//                Amount = 1000,
//                Instrument = "card",
//                Metadata = "Metadata",
//                OrderId = 2,
//                VendorId = 2,
//                Response = "Response",
//                Status = "succeeded",
//                TransactionType = "Auth",
//                TransactionTime = DateTime.UtcNow

//            };
//            var transactions = new List<TransactionDTO>() { transactionFirst, transactionSecond };
//            await context.Transactions.AddRangeAsync(transactionFirst, transactionSecond);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);
//            var actual = await (await repository.GetTransactions(2, 2, 2)).ToListAsync();
           
//            var expected = transactions;

//            //assert
//            for (int i = 0; i < transactions.Count; i++)
//            {
//                Assert.IsTrue(actual[0].TransactionId == expected[0].TransactionId);
//                Assert.IsTrue(actual[0].ExternalId == expected[0].ExternalId);
//                Assert.IsTrue(actual[0].UserId == expected[0].UserId);
//                Assert.IsTrue(actual[0].Amount == expected[0].Amount);
//                Assert.IsTrue(actual[0].Instrument == expected[0].Instrument);
//                Assert.IsTrue(actual[0].Metadata == expected[0].Metadata);
//                Assert.IsTrue(actual[0].OrderId == expected[0].OrderId);
//                Assert.IsTrue(actual[0].VendorId == expected[0].VendorId);
//                Assert.IsTrue(actual[0].Response == expected[0].Response);
//                Assert.IsTrue(actual[0].Status == expected[0].Status);
//                Assert.IsTrue(actual[0].TransactionType == expected[0].TransactionType);
//                Assert.IsTrue(actual[0].TransactionTime == expected[0].TransactionTime);
//            }
//        }
//        [TestMethod]
//        public async Task CreateUser_ValidUser_Succeeded()
//        {
//            //arrange
//            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
//                .UseInMemoryDatabase(databaseName: "CreateUser_ValidUser_Succeeded").Options;

//            var context = new PaymentsDbContext(options);

//            var user = new UserDTO()
//            {
//                Email = "test@mail.com",
//                ExternalId = "ch_externalidtest",
//                UserToken = "tok_userToken"

//            };
//            await context.Users.AddAsync(user);
//            await context.SaveChangesAsync();

//            //act 
//            var repository = new PaymentRepository(context);
//            var actual = await repository.GetUserByEmail(user.Email);
//            var expected = new UserDTO()
//            {
//                UserId = 1,
//                Email = "test@mail.com",
//                ExternalId = "ch_externalidtest",
//                UserToken = "tok_userToken"
//            };


//            //assert 
//            if (actual == null) Assert.Fail();
//            Assert.IsTrue(actual.Email == expected.Email);
//            Assert.IsTrue(actual.ExternalId == expected.ExternalId);
//            Assert.IsTrue(actual.UserId == expected.UserId);
//            Assert.IsTrue(actual.UserToken == expected.UserToken);
//        }
//    }
//}
