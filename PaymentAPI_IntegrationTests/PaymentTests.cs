using System.Net.Http;
using System.Threading.Tasks;
using BLL.Models;
using PaymentAPI;
using Xunit;

namespace PaymentAPI_IntegrationTests
{
    public class PaymentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public PaymentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ChargePaymentAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    CardToken = "tok_visa",
                    Currency = "usd",
                    Amount = 100000,
                    UserId = 7,
                    OrderId = 8,
                    VendorId = 7,
                    Email = "test@mail.com",
                    SaveCard = true,
                    Type = "charge"
                }
            };

            // Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task RefundPaymentAsync()
        {
            // Arrange
            var requestCharge = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    CardToken = "tok_visa",
                    Currency = "usd",
                    Amount = 100000,
                    UserId = 7,
                    OrderId = 8,
                    VendorId = 7,
                    Email = "test@mail.com",
                    SaveCard = true,
                    Type = "charge"
                }
            };

            var requestRefund = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    OrderId = 8,
                    Type = "refund"
                }
            };
            // Act
            await _client.PostAsync(requestCharge.Url, ContentHelper.GetStringContent(requestCharge.Body));
            var response = await _client.PostAsync(requestRefund.Url, ContentHelper.GetStringContent(requestRefund.Body));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task AuthPaymentAsync()
        {
            // Arrange
            var requestAuth = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    CardToken = "tok_visa",
                    Currency = "usd",
                    Amount = 100000,
                    UserId = 7,
                    OrderId = 8,
                    VendorId = 7,
                    Email = "test@mail.com",
                    SaveCard = true,
                    Type = "auth"
                }
            };

            var response = await _client.PostAsync(requestAuth.Url, ContentHelper.GetStringContent(requestAuth.Body));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CapturePaymentAsync()
        {
            // Arrange
            var requestAuth = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    CardToken = "tok_visa",
                    Currency = "usd",
                    Amount = 100000,
                    UserId = 7,
                    OrderId = 8,
                    VendorId = 7,
                    Email = "test@mail.com",
                    SaveCard = true,
                    Type = "auth"
                }
            };

            var requestCapture = new
            {
                Url = "/api/payment",
                Body = new PaymentModel
                {
                    OrderId = 8,
                    Type = "capture"
                }
            };
            // Act
            await _client.PostAsync(requestAuth.Url, ContentHelper.GetStringContent(requestAuth.Body));
            var response = await _client.PostAsync(requestCapture.Url, ContentHelper.GetStringContent(requestCapture.Body));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetAllInfoPaymentAsync()
        {
            // Arrange
            var requestAuth = new
            {
                Url = "/api/payment",
            };

            // Act
            var response = await _client.GetAsync(requestAuth.Url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetInfoPaymentByOrderAsync()
        {
            // Arrange
            var requestAuth = new
            {
                Url = "/api/payment?orderid=8",
            };

            // Act
            var response = await _client.GetAsync(requestAuth.Url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetInfoPaymentByOrderAndUserAsync()
        {
            // Arrange
            var requestAuth = new
            {
                Url = "/api/payment?orderid=8",
            };

            // Act
            var response = await _client.GetAsync(requestAuth.Url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}