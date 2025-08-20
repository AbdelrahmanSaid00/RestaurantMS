namespace MexicoRestaurant.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; } // An order item belongs to an order
        public int ProductId { get; set; }
        public Product? Product { get; set; } // An order item is for a specific product
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}