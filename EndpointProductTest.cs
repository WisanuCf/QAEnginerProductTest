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

        [TestFixture]
        public class DeleteApiTests
        {
    private RestClient _client;

    [SetUp]
    public void SetUp()
    {
        _client = new RestClient("https://fakestoreapi.com");
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public void Test_DeleteProduct_ReturnsOK()  // แก้ไขชื่อ Test ให้ตรงกับสิ่งที่ทดสอบ
    {
        // Arrange
        var productId = 6;
        var request = new RestRequest($"/products/{productId}", Method.Delete);

        // ก่อนการลบ แสดงข้อมูลผลิตภัณฑ์
        var getRequest = new RestRequest($"/products/{productId}", Method.Get);
        var getResponse = _client.Execute(getRequest);

//        Console.WriteLine("Before Deleting Product:");
//        Console.WriteLine(getResponse.Content); // แสดงข้อมูลผลิตภัณฑ์ที่ต้องการลบ

        // Act
        var response = _client.Execute(request);

        // หลังจากลบ แสดงผลลัพธ์
//        if (response.IsSuccessful)
//        {
//            Console.WriteLine($"Product with ID {productId} deleted successfully.");
//        }
//        else
//        {
//            Console.WriteLine($"Failed to delete product with ID {productId}. Status Code: {response.StatusCode}");
//        }

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Expected OK response.");  // เปลี่ยนจาก NoContent เป็น OK

        // ตรวจสอบข้อมูลหลังจากลบ (ทำการดึงข้อมูลซ้ำอีกครั้ง)
        var checkDeletedRequest = new RestRequest($"/products/{productId}", Method.Get);
        var checkDeletedResponse = _client.Execute(checkDeletedRequest);

//        Console.WriteLine("After Deleting Product:");
//        Console.WriteLine(checkDeletedResponse.Content); // ถ้าผลลัพธ์เป็น 404 แสดงว่าไม่พบข้อมูลหลังการลบ
    }
    }

        [TestFixture]
        public class CategoriesApiTests
        {
        private RestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient("https://fakestoreapi.com");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public void GetProductCategories_ShouldReturnListOfCategories()
        {
            // Arrange
            var request = new RestRequest("/products/categories", Method.Get);

            // Act
            var response = _client.Execute(request);

            // Assert
            Assert.That(response.IsSuccessful, Is.True, "API request was not successful.");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Unexpected HTTP status code.");
            Assert.That(response.Content, Is.Not.Empty, "Response content should not be empty.");

            List<string> categories = JsonSerializer.Deserialize<List<string>>(response.Content) ?? new List<string>();  // Ensure not null

            Assert.That(categories, Is.Not.Null.And.Not.Empty, "Failed to deserialize categories.");
//            Console.WriteLine(JsonSerializer.Serialize(categories));
        }
    }

        [TestFixture]
        public class SortProductApiTests
        {
    private RestClient _client;

    [SetUp]
    public void SetUp()
    {
        _client = new RestClient("https://fakestoreapi.com");
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public void GetProductsSortedByIdDescending_ShouldReturnProductsInDescendingOrder()
    {
        // Arrange
        var request = new RestRequest("/products?sort=desc", Method.Get);

        // Act
        var response = _client.Execute(request);

        // Assert
        Assert.That(response.IsSuccessful, Is.True, "API request was not successful.");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Unexpected HTTP status code.");
        Assert.That(response.Content, Is.Not.Empty, "Response content should not be empty.");

        // Deserialize JSON response to Product list
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        List<Product> products = JsonSerializer.Deserialize<List<Product>>(response.Content, options) ?? new List<Product>();

        Assert.That(products, Is.Not.Null.And.Not.Empty, "Failed to deserialize products.");

        // Check if products are sorted by ID in descending order
        for (int i = 1; i < products.Count; i++)
        {
            Assert.That(products[i].Id, Is.LessThanOrEqualTo(products[i - 1].Id),
                $"Products are not sorted correctly by ID. Found {products[i].Id} before {products[i - 1].Id}");
        }
    }
    }
    }
}