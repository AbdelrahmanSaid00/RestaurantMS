using MexicoRestaurant.Models;

namespace MexicoRestaurant.ViewModel
{
    public class OrderVM
    {
        public decimal TotalAmount { get; set; }
        public List<OrderItems> orderItems { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
