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
            int actual = 0;
            bool expected = true;
            Assert.AreEqual(expected, actual.IsZero());
        }

        [TestMethod]
        public void IsZero_NotZero_Success()
        {
            int actual = 1;
            bool expected = false;
            Assert.AreEqual(expected, actual.IsZero());
        }
    }
}