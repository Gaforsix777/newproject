// Ingredient.cs
using System.ComponentModel.DataAnnotations;

namespace PrimerParcial.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Quantity { get; set; } = string.Empty;

        // FK a Recipe (1 Recipe -> N Ingredients)
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
