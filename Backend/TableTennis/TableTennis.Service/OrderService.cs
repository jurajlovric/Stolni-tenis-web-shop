using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Repository.Common;
using TableTennis.Service.Common;

namespace TableTennis.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task AddOrderAsync(Order order)
        {
            if (order.UserId == Guid.Empty)
            {
                throw new Exception("Neispravan ID korisnika.");
            }

            Console.WriteLine($"Provjera korisnika s ID-jem: {order.UserId}");
            var user = await _userRepository.GetUserByIdAsync(order.UserId);
            if (user == null)
            {
                throw new Exception($"Korisnik s ID-jem {order.UserId} ne postoji.");
            }

            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "u obradi";

            order.TotalAmount = CalculateTotalAmount(order.OrderItems);

            foreach (var item in order.OrderItems)
            {
                item.OrderItemId = Guid.NewGuid();
                item.OrderId = order.OrderId;

                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Proizvod s ID-jem {item.ProductId} ne postoji.");
                }

                item.Price = product.Price;
                if (item.Quantity <= 0)
                {
                    throw new Exception("Količina proizvoda mora biti veća od nule.");
                }
            }

            try
            {
                await _orderRepository.AddOrderAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri dodavanju narudžbe: {ex.Message}");
                throw new Exception("Došlo je do greške pri kreiranju narudžbe. Molimo pokušajte ponovo.");
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        private decimal CalculateTotalAmount(IEnumerable<OrderItem> orderItems)
        {
            decimal total = 0;
            foreach (var item in orderItems)
            {
                total += item.Price * item.Quantity;
            }
            return total;
        }
    }
}
