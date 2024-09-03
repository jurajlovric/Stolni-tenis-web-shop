using System;

namespace TableTennis.Model
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }  
        public decimal Price { get; set; } 
        public DateTime OrderDate { get; set; } 
        public string Status { get; set; } 
        public decimal TotalAmount { get; set; }  
        public User User { get; set; }           
        public Product Product { get; set; }  
    }
}
