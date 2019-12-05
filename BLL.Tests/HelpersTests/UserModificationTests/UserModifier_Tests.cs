using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Helpers.Queries.Interfaces;
using BLL.Helpers.UserUpdating;
using BLL.Models;
using BLL.Tests.Constants;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLL.Tests.HelpersTests.UserModification_Tests
{
    [TestClass]
    public class UserModifier_Tests
    {
        Mock<IUserRepository> _userRepositoryMock;
        Mock<IUserQueryCreator> _userQueryCreatorMock;

        Mock<UserModifier> userModifier;

        [TestInitialize()]
        public void Initializer()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userQueryCreatorMock = new Mock<IUserQueryCreator>();

            _userQueryCreatorMock.Setup(x => x.GetUser(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Expression<Func<UserDTO, bool>>>());

            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<Expression<Func<UserDTO, bool>>>()))
                .ReturnsAsync(new List<UserDTO>() { TestConstants.user });

            _userRepositoryMock.Setup(x => x.CreateUser(It.IsAny<UserDTO>()))
                .ReturnsAsync(It.IsAny<UserDTO>());

            userModifier = new Mock<UserModifier>(
                _userRepositoryMock.Object,
                _userQueryCreatorMock.Object
                );
        }

        [TestMethod]
        public async Task GetOrCreateUser_ValidModels_Success()
        {
            //Act
            var actual = await userModifier.Object.GetOrCreateUser(TestConstants.payment);

            var expected = TestConstants.user;

            //Assert
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.ExternalId, actual.ExternalId);
            Assert.AreEqual(expected.UserToken, actual.UserToken);
        }
    }
}
