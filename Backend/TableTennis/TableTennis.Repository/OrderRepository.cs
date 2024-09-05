using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using TableTennis.Model;
using TableTennis.Repository.Common;

namespace TableTennis.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddOrderAsync(Order order)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        var orderCommand = new NpgsqlCommand(
                            "INSERT INTO \"Order\" (order_id, user_id, order_date, status, total_amount) " +
                            "VALUES (@OrderId, @UserId, @OrderDate, @Status, @TotalAmount)", conn, transaction);

                        orderCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
                        orderCommand.Parameters.AddWithValue("@UserId", order.UserId);
                        orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        orderCommand.Parameters.AddWithValue("@Status", order.Status);
                        orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                        await orderCommand.ExecuteNonQueryAsync();

                        foreach (var item in order.OrderItems)
                        {
                            var itemCommand = new NpgsqlCommand(
                                "INSERT INTO \"OrderItem\" (order_item_id, order_id, product_id, quantity, price) " +
                                "VALUES (@OrderItemId, @OrderId, @ProductId, @Quantity, @Price)", conn, transaction);

                            itemCommand.Parameters.AddWithValue("@OrderItemId", item.OrderItemId);
                            itemCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
                            itemCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                            itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                            itemCommand.Parameters.AddWithValue("@Price", item.Price);

                            await itemCommand.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = new List<Order>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var commandText = @"
                    SELECT o.order_id, o.user_id, o.order_date, o.status, o.total_amount,
                           oi.order_item_id, oi.product_id, oi.quantity, oi.price, p.name AS product_name
                    FROM ""Order"" o
                    LEFT JOIN ""OrderItem"" oi ON o.order_id = oi.order_id
                    LEFT JOIN ""Product"" p ON oi.product_id = p.product_id;";

                using (var command = new NpgsqlCommand(commandText, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var orderId = reader.GetGuid(reader.GetOrdinal("order_id"));
                        var order = orders.Find(o => o.OrderId == orderId);

                        if (order == null)
                        {
                            order = new Order
                            {
                                OrderId = orderId,
                                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount")),
                                OrderItems = new List<OrderItem>()
                            };
                            orders.Add(order);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("order_item_id")))
                        {
                            order.OrderItems.Add(new OrderItem
                            {
                                OrderItemId = reader.GetGuid(reader.GetOrdinal("order_item_id")),
                                OrderId = orderId,
                                ProductId = reader.GetGuid(reader.GetOrdinal("product_id")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price")),
                                ProductName = reader.GetString(reader.GetOrdinal("product_name"))
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = new List<Order>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var commandText = @"
                    SELECT o.order_id, o.user_id, o.order_date, o.status, o.total_amount,
                           oi.order_item_id, oi.product_id, oi.quantity, oi.price, p.name AS product_name
                    FROM ""Order"" o
                    LEFT JOIN ""OrderItem"" oi ON o.order_id = oi.order_id
                    LEFT JOIN ""Product"" p ON oi.product_id = p.product_id
                    WHERE o.user_id = @UserId;";

                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var orderId = reader.GetGuid(reader.GetOrdinal("order_id"));
                            var order = orders.Find(o => o.OrderId == orderId);

                            if (order == null)
                            {
                                order = new Order
                                {
                                    OrderId = orderId,
                                    UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                                    Status = reader.GetString(reader.GetOrdinal("status")),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount")),
                                    OrderItems = new List<OrderItem>()
                                };
                                orders.Add(order);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("order_item_id")))
                            {
                                order.OrderItems.Add(new OrderItem
                                {
                                    OrderItemId = reader.GetGuid(reader.GetOrdinal("order_item_id")),
                                    OrderId = orderId,
                                    ProductId = reader.GetGuid(reader.GetOrdinal("product_id")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                                    ProductName = reader.GetString(reader.GetOrdinal("product_name"))
                                });
                            }
                        }
                    }
                }
            }

            return orders;
        }
    }
}
