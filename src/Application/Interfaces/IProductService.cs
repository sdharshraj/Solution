using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> AddAsync(ProductDto productDto);
        Task UpdateAsync(int id, ProductDto productDto);
        Task DeleteAsync(int id);
        Task<List<ProductWithItemCount>> GetProductsWithItemCountAsync();
    }
}
