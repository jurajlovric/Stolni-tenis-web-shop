using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Repository.Common;
using TableTennis.Service.Common;

namespace TableTennis.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Dohvati sve proizvode
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        // Dohvati proizvod po ID-u
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        // Dodaj novi proizvod
        public async Task AddProductAsync(Product product)
        {
            product.ProductId = Guid.NewGuid(); // Generira novi GUID za proizvod
            await _productRepository.AddProductAsync(product);
        }

        // Ažuriraj postojeći proizvod
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
        }

        // Izbriši proizvod po ID-u
        public async Task DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
