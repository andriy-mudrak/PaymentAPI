
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BLL.Helpers.Queries;
using BLL.Models;
using BLL.Tests.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLL.Tests.HelpersTests.QueriesTests
{
    [TestClass]
    public class UserQueryCreator_Tests
    {
        [TestMethod]
        public async Task GetUser_ValidModels_Success()
        {
            //Arrange
            var queryCreator = new UserQueryCreator();
            var filter = await queryCreator.GetUser(TestConstants.user.Email);
            
            //Act
            var actual = TestConstants.users.Select(x=>x).Where(filter.Compile()).LastOrDefault();
            var expected = TestConstants.users.LastOrDefault();

            //Assert
            Assert.AreEqual(actual.UserId, expected.UserId);
            Assert.AreEqual(actual.ExternalId, expected.ExternalId);
            Assert.AreEqual(actual.Email, expected.Email);
            Assert.AreEqual(actual.UserToken, expected.UserToken);
        }
    }
}