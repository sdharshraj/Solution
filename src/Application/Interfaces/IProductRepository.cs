using Application.DTOs;
using Domain.Entities;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product, int initialQuantity = 0);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<List<ProductWithItemCount>> GetProductsWithItemCountAsync();
}
