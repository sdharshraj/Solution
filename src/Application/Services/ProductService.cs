using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CreatedBy = p.CreatedBy,
                CreatedOn = p.CreatedOn,
                ModifiedBy = p.ModifiedBy,
                ModifiedOn = p.ModifiedOn
            });
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn
            };
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                CreatedBy = productDto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task UpdateAsync(int id, ProductDto productDto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.ProductName = productDto.ProductName;
            product.ModifiedBy = productDto.ModifiedBy;
            product.ModifiedOn = DateTime.UtcNow;

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<ProductWithItemCount>> GetProductsWithItemCountAsync()
        {
            var products = await _repository.GetProductsWithItemCountAsync();

            return products.Select(p => new ProductWithItemCount
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ItemCount = p.ItemCount,
                TotalQuantity = p.TotalQuantity
            }).ToList();
        }
    }
}
