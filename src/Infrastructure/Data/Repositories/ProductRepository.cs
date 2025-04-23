using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FindAsync(id);

    public async Task AddAsync(Product product, int initialQuantity = 0)
    {
        // Add product to the database
        _context.Products.Add(product);
        await _context.SaveChangesAsync();  // Save the new product

        // Create the associated Item (e.g., initial inventory)
        var item = new Item
        {
            ProductId = product.Id,  // The ProductId links this item to the product
            Quantity = initialQuantity // Set the quantity to the desired initial value
        };

        _context.Items.Add(item);  // Add the new item
        await _context.SaveChangesAsync();  // Save the item
    }

    public async Task UpdateAsync(Product product)
    {
        // Update product in the database
        _context.Products.Update(product);
        await _context.SaveChangesAsync();  // Save changes
        
        // If needed, you can update item data related to the product
        var item = await _context.Items.FirstOrDefaultAsync(i => i.ProductId == product.Id);
        if (item != null)
        {
            // Example of updating quantity if applicable (can be customized as per requirement)
            // item.Quantity = product.Quantity;  
            _context.Items.Update(item);
            await _context.SaveChangesAsync();  // Save item update
        }
    }

    public async Task DeleteAsync(int id)
    {
        // Find the product to be deleted
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            // Delete the related Item using the foreign key relation
            var item = await _context.Items.FirstOrDefaultAsync(i => i.ProductId == product.Id);
            if (item != null)
            {
                _context.Items.Remove(item);  // Remove the related item
            }

            // Remove the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();  // Save the deletion
        }
    }

    public async Task<List<ProductWithItemCount>> GetProductsWithItemCountAsync()
    {
        // Fetch products with associated item count and total quantity
        var result = await _context.Products
            .Select(p => new ProductWithItemCount
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                ItemCount = _context.Items.Where(i => i.ProductId == p.Id).Count(),
                TotalQuantity = _context.Items.Where(i => i.ProductId == p.Id).Sum(i => (int?)i.Quantity) ?? 0
            })
            .ToListAsync();

        return result;
    }
}
