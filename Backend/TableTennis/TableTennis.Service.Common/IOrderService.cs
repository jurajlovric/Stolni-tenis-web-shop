using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableTennis.Model;

namespace TableTennis.Service.Common
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    }
}
