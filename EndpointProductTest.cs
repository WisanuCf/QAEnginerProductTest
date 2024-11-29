using NUnit.Framework;
using RestSharp;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTests
{
    [TestFixture]
    public class ProductApiTests
    {
        private RestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient("https://fakestoreapi.com"); // Replace with your API base URL
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public void Test_GetProducts_ReturnsOk()
        {
            // Arrange
            var request = new RestRequest("/products", Method.Get);

            // Act
            var response = _client.Execute(request);

            // Assert
            Assert.That(response.IsSuccessful, Is.True, "API request failed.");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Unexpected HTTP status code.");
            Assert.That(response.Content, Is.Not.Empty, "Response content should not be empty.");
        }
    }
}