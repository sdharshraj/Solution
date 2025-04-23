using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Product/withItemCount
        [HttpGet("withItemCount")]
        public async Task<ActionResult<IEnumerable<ProductWithItemCount>>> GetProductsWithItemCount()
        {
            var products = await _productService.GetProductsWithItemCountAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            var product = await _productService.AddAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }
            await _productService.UpdateAsync(id, productDto);
            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
// This code defines a ProductController class that handles HTTP requests related to the Product entity. It uses dependency injection to access the IProductService interface, which provides methods for CRUD operations on products. The controller includes actions for getting all products, getting a product by ID, creating a new product, updating an existing product, and deleting a product. Each action returns appropriate HTTP status codes and responses based on the outcome of the operation.
// The controller is decorated with the [Route] and [ApiController] attributes to define the routing and behavior of the API.