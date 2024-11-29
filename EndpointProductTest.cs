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
        public void Test1()
        {
            Assert.Pass();
        }
    }
}