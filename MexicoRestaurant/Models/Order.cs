namespace MexicoRestaurant.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } // An order is placed by a user
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } // An order can have many orderitems
    }
}
