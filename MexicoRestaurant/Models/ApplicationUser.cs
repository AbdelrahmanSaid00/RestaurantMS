using Microsoft.AspNetCore.Identity;

namespace MexicoRestaurant.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
