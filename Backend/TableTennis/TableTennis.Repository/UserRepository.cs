using System;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Repository.Common;
using Npgsql;

namespace TableTennis.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var query = @"
                    SELECT user_id, username, email, password, created_at, role_id
                    FROM ""User""
                    WHERE user_id = @UserId;";

                using (var command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("role_id"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
                INSERT INTO ""User"" 
                (user_id, username, email, password, created_at, role_id) 
                VALUES 
                (@UserId, @Username, @Email, @Password, @CreatedAt, @RoleId) 
                RETURNING *;";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
            command.Parameters.AddWithValue("@RoleId", user.RoleId);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    RoleId = reader.GetGuid(reader.GetOrdinal("role_id"))
                };
            }

            return null;
        }

        public async Task<Guid?> GetRoleIdByUsernameAsync(string username)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT ""role_id""
                FROM ""User""
                WHERE ""username"" = @Username;";

            await using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);

            var roleId = await command.ExecuteScalarAsync() as Guid?;
            return roleId;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = @"
            SELECT user_id, username, email, password, created_at, role_id
            FROM ""User""
            WHERE email = @Email;";

                using (var command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("role_id"))
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}
