using System.Threading.Tasks;
using BLL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Tests.HelpersTests.ExtensionsTests
{
    [TestClass]
    public class Int64Extension_Tests
    {
        [TestMethod]
        public void IsZero_Zero_Success()
        {
            //Arrange
            int zero = 0;
            //Act
            bool actual = zero.IsZero();
            bool expected = true;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsZero_NotZero_Success()
        {
            //Arrange
            int notZero = 0;
            //Act
            bool actual = notZero.IsZero();
            bool expected = true;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}