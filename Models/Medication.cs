using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Inventory_System.Models
{
    public class Medication
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medication name is required.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string Image { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Price must be zero or greater.")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be zero or greater.")]
        public int StockQuantity { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
