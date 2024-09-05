using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using TableTennis.Model;
using TableTennis.Repository.Common;

namespace TableTennis.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "SELECT category_id, name, description FROM \"Category\";";
                using (var command = new NpgsqlCommand(commandText, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var category = new Category
                        {
                            CategoryId = reader.GetGuid(reader.GetOrdinal("category_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("description"))
                        };
                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            Category category = null;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "SELECT category_id, name, description FROM \"Category\" WHERE category_id = @id;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            category = new Category
                            {
                                CategoryId = reader.GetGuid(reader.GetOrdinal("category_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("description"))
                            };
                        }
                    }
                }
            }

            return category;
        }

        public async Task AddCategoryAsync(Category category)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "INSERT INTO \"Category\" (category_id, name, description) VALUES (@id, @name, @description);";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", category.CategoryId);
                    command.Parameters.AddWithValue("@name", category.Name);
                    command.Parameters.AddWithValue("@description", (object)category.Description ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "UPDATE \"Category\" SET name = @name, description = @description WHERE category_id = @id;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", category.CategoryId);
                    command.Parameters.AddWithValue("@name", category.Name);
                    command.Parameters.AddWithValue("@description", (object)category.Description ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = "DELETE FROM \"Category\" WHERE category_id = @id;";
                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
