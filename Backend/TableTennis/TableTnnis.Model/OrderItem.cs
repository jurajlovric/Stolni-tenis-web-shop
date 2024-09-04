using System;

namespace TableTennis.Model
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ProductName { get; set; } // Postavite kao nullable, jer nije obavezno prilikom POST zahtjeva
    }
}
