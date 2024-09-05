using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Service.Common;

namespace TableTennis.WebApi.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju proizvoda.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            Console.WriteLine($"Primljeni podaci: Name={product.Name}, Price={product.Price}, CategoryId={product.CategoryId}, ImageUrl={product.ImageUrl}");

            // Provjera je li CategoryId ispravno postavljen
            if (product.CategoryId == Guid.Empty)
            {
                Console.WriteLine("CategoryId nije ispravno postavljen.");
                return BadRequest("CategoryId is required.");
            }

            // Provjera ostalih podataka
            if (string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                Console.WriteLine("Ime proizvoda ili cijena nisu ispravni.");
                return BadRequest("Ime proizvoda i cijena moraju biti ispravni.");
            }

            try
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri dodavanju proizvoda: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
