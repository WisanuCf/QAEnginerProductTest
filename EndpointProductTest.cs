using NUnit.Framework;
using RestSharp;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTests
{

    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }

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

        [Test]
        public void Test_CheckProductFormat()
        {
            // Arrange
            var request = new RestRequest("/products", Method.Get);

            // Act
            var response = _client.Execute(request);

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content is null or empty.");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Product> products = JsonSerializer.Deserialize<List<Product>>(response.Content, options) ?? new List<Product>();  // Ensure not null

            // Assert
            Assert.That(products.Count, Is.LessThanOrEqualTo(20), "Expected at most 20 products.");
        }


    }
}