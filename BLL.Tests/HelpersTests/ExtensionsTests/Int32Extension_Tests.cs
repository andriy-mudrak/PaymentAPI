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