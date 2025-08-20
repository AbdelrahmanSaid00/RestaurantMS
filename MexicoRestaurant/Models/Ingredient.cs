namespace MexicoRestaurant.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; } // Unique identifier for the ingredient
        public string? Name { get; set; } // Name of the ingredient
        public ICollection<ProductIngredient> ProductIngredients { get; set; } // A collection of product-ingredient relationships
    }
}