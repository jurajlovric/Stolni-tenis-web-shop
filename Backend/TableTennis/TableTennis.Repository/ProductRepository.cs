using Npgsql;
using TableTennis.Model;
using TableTennis.Repository.Common;

namespace TableTennis.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Dohvati sve proizvode
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = @"
                    SELECT p.product_id, p.name, p.description, p.price, p.category_id, c.name AS category_name, p.image_url
                    FROM ""Product"" p
                    JOIN ""Category"" c ON p.category_id = c.category_id;"; // Dodan JOIN za dohvaćanje naziva kategorije
                using (var command = new NpgsqlCommand(commandText, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            ProductId = reader.GetGuid(reader.GetOrdinal("product_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("price")),
                            CategoryId = reader.GetGuid(reader.GetOrdinal("category_id")),
                            CategoryName = reader.GetString(reader.GetOrdinal("category_name")), // Povlači ime kategorije
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("image_url"))
                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        // Dohvati proizvod po ID-u
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            Product product = null;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = @"
                    SELECT p.product_id, p.name, p.description, p.price, p.category_id, c.name AS category_name, p.image_url
                    FROM ""Product"" p
                    JOIN ""Category"" c ON p.category_id = c.category_id
                    WHERE p.product_id = @id;"; // Dodan JOIN za dohvaćanje naziva kategorije
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("product_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price")),
                                CategoryId = reader.GetGuid(reader.GetOrdinal("category_id")),
                                CategoryName = reader.GetString(reader.GetOrdinal("category_name")), // Povlači ime kategorije
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("image_url"))
                            };
                        }
                    }
                }
            }

            return product;
        }

        // Dodaj novi proizvod
        public async Task AddProductAsync(Product product)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "INSERT INTO \"Product\" (product_id, name, description, price, category_id, image_url) VALUES (@id, @name, @description, @price, @categoryId, @imageUrl);";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", product.ProductId);
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@description", (object)product.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@categoryId", product.CategoryId);
                    command.Parameters.AddWithValue("@imageUrl", (object)product.ImageUrl ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Ažuriraj postojeći proizvod
        public async Task UpdateProductAsync(Product product)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "UPDATE \"Product\" SET name = @name, description = @description, price = @price, category_id = @categoryId, image_url = @imageUrl WHERE product_id = @id;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", product.ProductId);
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@description", (object)product.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@categoryId", product.CategoryId);
                    command.Parameters.AddWithValue("@imageUrl", (object)product.ImageUrl ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Izbriši proizvod po ID-u
        public async Task DeleteProductAsync(Guid id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "DELETE FROM \"Product\" WHERE product_id = @id;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
