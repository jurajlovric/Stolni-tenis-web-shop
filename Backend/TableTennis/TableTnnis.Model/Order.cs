using System;
using System.Collections.Generic;

namespace TableTennis.Model
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "u obradi";
        public decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
