using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Models
{
    public class FoodItem
    {
        [Required]
        public int FoodItemId { get; set; }

        [Required]
        public string FoodItemName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float UnitPrice { get; set; }
    }
}
