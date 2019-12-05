using System;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Helpers.Mapping;
using BLL.Models;
using BLL.Tests.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Stripe;

namespace BLL.Tests.HelpersTests.ExtensionsTests
{
    [TestClass]
    public class Int32Extension_Tests
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