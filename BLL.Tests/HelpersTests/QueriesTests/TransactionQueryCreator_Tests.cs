using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers.Queries;
using BLL.Tests.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Tests.HelpersTests.QueriesTests
{
    [TestClass]
    public class TransactionQueryCreator_Tests
    {

        [TestMethod]
        public async Task GetAllTransactions_ValidModels_Success()
        {
            //Arrange
            var queryCreator = new TransactionQueryCreator();
            var transaction = TestConstants.transactionsDTO.LastOrDefault();
            var filter = await queryCreator.GetAllTransactions(transaction.OrderId, transaction.UserId);

            //Act
            var actual = TestConstants.transactionsDTO.Select(x => x).Where(filter.Compile()).LastOrDefault();
            var expected = TestConstants.transactionsDTO.LastOrDefault();

            //Assert
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.ExternalId, actual.ExternalId);
            Assert.AreEqual(expected.Instrument, actual.Instrument);
            Assert.AreEqual(expected.Metadata, actual.Metadata);
            Assert.AreEqual(expected.OrderId, actual.OrderId);
            Assert.AreEqual(expected.Response, actual.Response);
            Assert.AreEqual(expected.Status, actual.Status);
            Assert.AreEqual(expected.TransactionTime, actual.TransactionTime);
            Assert.AreEqual(expected.TransactionType, actual.TransactionType);
            Assert.AreEqual(expected.VendorId, actual.VendorId);
        }
    }
}