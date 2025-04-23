using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnProductDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                ProductName = "Test Product",
                CreatedBy = "Test User",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = "Test User",
                ModifiedOn = DateTime.UtcNow
            }
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
                       .ReturnsAsync(products);
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().ProductName.Should().Be("Test Product");
    }
}
