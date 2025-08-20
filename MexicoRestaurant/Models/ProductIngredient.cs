namespace MexicoRestaurant.Models
{
    public class ProductIngredient
    {
        public int ProductId { get; set; } // Foreign key to Product
        public Product Product { get; set; } // Navigation property to Product
        public int IngredientId { get; set; } // Foreign key to Ingredient
        public Ingredient? Ingredient { get; set; } // Navigation property to Ingredient
    }
}