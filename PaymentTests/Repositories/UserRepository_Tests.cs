using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DBModels;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaymentTests
{
    [TestClass]
    public class UserRepository_Tests
    {
        private UserDTO _user;

        [TestInitialize()]
        public void Initializer()
        {
            _user = new UserDTO()
            {
                Email = "test@mail.com",
                ExternalId = "cus_AsdewW23sSkasdkval2",
                UserToken = "tok_ldaHKJhkdjBJASbj2"
            };
        }

        [TestMethod]
        public async Task CreateUser_ValidModel_Success()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateUser_ValidModel_Success")
                .Options;

            using (var context = new PaymentsDbContext(options))
            {
                var service = new UserRepository(context);
                await service.CreateUser(_user);
                context.SaveChanges();
            }

            //Act
            UserDTO actual;
            using (var context = new PaymentsDbContext(options))
            {
                actual = await context.Users.Select(x => x).Where(x => x.Email == _user.Email).LastOrDefaultAsync();
            }
            var expected = _user;

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Email, expected.Email);
            Assert.AreEqual(actual.UserToken, expected.UserToken);
        }

        [TestMethod]
        public async Task GetUser_ValidModel_Success()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
                .UseInMemoryDatabase(databaseName: "GetUser_ValidModel_Success")
                .Options;

            using (var context = new PaymentsDbContext(options))
            {
                await context.Users.AddAsync(_user);
                await context.SaveChangesAsync();
            }

            //Act
            UserDTO actual;
            using (var context = new PaymentsDbContext(options))
            {
                var service = new UserRepository(context);
                actual = (await service.GetUser(x => x.Email == _user.Email)).LastOrDefault();
            }
            var expected = _user;

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Email, expected.Email);
            Assert.AreEqual(actual.UserToken, expected.UserToken);
        }

        [TestMethod]
        public async Task UpdateUser_ValidModel_Success()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PaymentsDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUser_ValidModel_Success")
                .Options;

            using (var context = new PaymentsDbContext(options))
            {
                await context.Users.AddAsync(_user);
                await context.SaveChangesAsync();
            }

            var updateUserModel = new UserDTO()
            {
                Email = "test@mail.com",
                ExternalId = "cus_Akortasdtasdmv2",
                UserToken = "tok_SecretTokenForPayment"
            };

            //Act
            UserDTO actual;
            using (var context = new PaymentsDbContext(options))
            {
                var service = new UserRepository(context);
                await service.UpdateUser(updateUserModel);
            }

            using (var context = new PaymentsDbContext(options))
            {
                var service = new UserRepository(context);
                await service.UpdateUser(updateUserModel);
                actual = await context.Users.Select(x => x).Where(x => x.Email == _user.Email).LastOrDefaultAsync();
            }

            var expected = updateUserModel;

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Email, expected.Email);
            Assert.AreEqual(actual.UserToken, expected.UserToken);
        }
    }
}