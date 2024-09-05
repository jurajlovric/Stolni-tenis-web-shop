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

        // Dodaj novu narudžbu
        public async Task AddOrderAsync(Order order)
        {
            // Provjera je li userId prazan ili neispravan
            if (order.UserId == Guid.Empty)
            {
                throw new Exception("Neispravan ID korisnika.");
            }

            // Provjera postoji li korisnik s danim userId prije dodavanja narudžbe
            Console.WriteLine($"Provjera korisnika s ID-jem: {order.UserId}"); // Logiranje userId
            var user = await _userRepository.GetUserByIdAsync(order.UserId);
            if (user == null)
            {
                throw new Exception($"Korisnik s ID-jem {order.UserId} ne postoji.");
            }

            // Generiranje jedinstvenih ID-ova za narudžbu i stavke
            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "u obradi";

            // Izračun ukupnog iznosa narudžbe i provjera svakog proizvoda
            order.TotalAmount = CalculateTotalAmount(order.OrderItems);

            foreach (var item in order.OrderItems)
            {
                item.OrderItemId = Guid.NewGuid();
                item.OrderId = order.OrderId;

                // Dohvati cijenu proizvoda iz baze pomoću ProductRepository
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Proizvod s ID-jem {item.ProductId} ne postoji.");
                }

                // Postavljanje cijene iz baze i provjera količine
                item.Price = product.Price;
                if (item.Quantity <= 0)
                {
                    throw new Exception("Količina proizvoda mora biti veća od nule.");
                }
            }

            // Pokušaj dodavanja narudžbe u bazu
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
