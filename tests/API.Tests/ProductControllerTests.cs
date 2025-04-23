using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk()
        {
            // Act: Make the actual request to your API
            var response = await _client.GetAsync("/api/product");

            // Assert: Verify the response status
            response.EnsureSuccessStatusCode();  // This will throw an exception if the status code is not 2xx
        }
    }
}
