using TableTennis.Model;
using TableTennis.Repository.Common;
using Npgsql;

namespace TableTennis.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "SELECT * FROM \"Product\";";
                using (var command = new NpgsqlCommand(commandText, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            Product product = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "SELECT * FROM \"Product\" WHERE \"ProductId\" = @ProductId;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("ProductId", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };
                        }
                    }
                }
            }
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "INSERT INTO \"Product\" (\"ProductId\", \"Name\", \"Description\", \"Price\", \"CategoryId\", \"ImageUrl\") VALUES (@ProductId, @Name, @Description, @Price, @CategoryId, @ImageUrl);";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("ProductId", product.ProductId);
                    command.Parameters.AddWithValue("Name", product.Name);
                    command.Parameters.AddWithValue("Description", product.Description);
                    command.Parameters.AddWithValue("Price", product.Price);
                    command.Parameters.AddWithValue("CategoryId", product.CategoryId);
                    command.Parameters.AddWithValue("ImageUrl", product.ImageUrl);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "UPDATE \"Product\" SET \"Name\" = @Name, \"Description\" = @Description, \"Price\" = @Price, \"CategoryId\" = @CategoryId, \"ImageUrl\" = @ImageUrl WHERE \"ProductId\" = @ProductId;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("ProductId", product.ProductId);
                    command.Parameters.AddWithValue("Name", product.Name);
                    command.Parameters.AddWithValue("Description", product.Description);
                    command.Parameters.AddWithValue("Price", product.Price);
                    command.Parameters.AddWithValue("CategoryId", product.CategoryId);
                    command.Parameters.AddWithValue("ImageUrl", product.ImageUrl);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteProductAsync(Guid id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "DELETE FROM \"Product\" WHERE \"ProductId\" = @ProductId;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("ProductId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
