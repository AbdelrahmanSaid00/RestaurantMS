using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MexicoRestaurant.Models
{
    public class Product
    {
        public Product()
        {
            productIngredients = new List<ProductIngredient>();
        }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(10.00 , 300.00 , ErrorMessage = "Price must be between 10.00 and 300.00")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; } = string.Empty; // URL to the product image
        [ValidateNever]
        public Category? Category { get; set; } // A product belongs to a category
        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; } // A product can be part of many order items
        [ValidateNever]
        public ICollection<ProductIngredient>? productIngredients { get; set; } // A product can have many ingredients
    }
}