using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ProductRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        var product = new Product{
            ProductName = "Infra Test",
            CreatedBy = "Test User",
            CreatedOn = DateTime.UtcNow,
            ModifiedBy = "Test User",
            ModifiedOn = DateTime.UtcNow
        };

        await _repository.AddAsync(product);

        var saved = await _context.Products.FirstOrDefaultAsync();
        saved.Should().NotBeNull();
        saved.ProductName.Should().Be("Infra Test");
    }
}
