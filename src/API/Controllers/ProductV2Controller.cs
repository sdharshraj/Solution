using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class ProductV2Controller : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductV2Controller(IProductService productService)
		{
			_productService = productService;
		}

		// GET: api/v2/Product
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
		{
			var products = await _productService.GetAllAsync();
			return Ok(products);
		}

		// GET: api/v2/Product/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var product = await _productService.GetByIdAsync(id);
			if (product == null)
			{
				throw new KeyNotFoundException("Product not found.");
			}
			return Ok(product);
		}

		// POST: api/v2/Product
		[HttpPost]
		public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
		{
			var product = await _productService.AddAsync(productDto);
			return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
		}

		// PUT: api/v2/Product/{id}
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

		// DELETE: api/v2/Product/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			await _productService.DeleteAsync(id);
			return NoContent();
		}
	}
}
