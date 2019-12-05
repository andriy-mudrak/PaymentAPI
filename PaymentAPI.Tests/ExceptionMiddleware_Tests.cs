using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using NSubstitute;
using PaymentAPI.Helpers.Constants;
using PaymentAPI.Middleware;
using PaymentAPI.Models;
using Serilog;

namespace PaymentAPI.Tests
{
    [TestClass]
    public class ExceptionMiddleware_Tests
    {
        [TestMethod]
        public async Task InvokeAsync_ThrowException_CatchExceptionAndWriteMessage()
        {
            // Arrange
            var middleware = new ExceptionMiddleware(next: (innerHttpContext) =>
            {
                throw new Exception("Test");
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ErrorInfo>(streamText);

            //Assert
            Assert.AreEqual(objResponse.Message, MiddlewareConstants.STANDART_ERROR_MESSAGE);
        }
    }
}
