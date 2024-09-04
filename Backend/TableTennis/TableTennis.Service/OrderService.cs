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
        private readonly IProductRepository _productRepository; // Dodana ovisnost o ProductRepository

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // Dodaj novu narudžbu
        public async Task AddOrderAsync(Order order)
        {
            // Generiranje jedinstvenih ID-ova za narudžbu i stavke
            order.OrderId = Guid.NewGuid();

            foreach (var item in order.OrderItems)
            {
                item.OrderItemId = Guid.NewGuid();
                item.OrderId = order.OrderId; // Postavljanje OrderId za svaku stavku narudžbe

                // Dohvati cijenu proizvoda iz baze pomoću ProductRepository
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Proizvod s ID-jem {item.ProductId} ne postoji.");
                }

                item.Price = product.Price; // Postavljanje cijene proizvoda iz baze
            }

            // Postavljanje datuma narudžbe na trenutni datum i vrijeme
            order.OrderDate = DateTime.UtcNow;

            // Izračun ukupnog iznosa narudžbe
            order.TotalAmount = CalculateTotalAmount(order.OrderItems);

            // Postavljanje statusa narudžbe
            order.Status = "u obradi";

            await _orderRepository.AddOrderAsync(order);
        }

        // Dohvati sve narudžbe
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        // Dohvati narudžbe po ID-u korisnika
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        // Pomoćna metoda za izračun ukupnog iznosa narudžbe
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
