using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Product
    {
        [Key]
        public long? ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        [Range(0, 999999.99, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [Range(0, 999999.99, ErrorMessage = "Rent price must be a positive number")]
        public decimal? RentPrice { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; } = null!;

        public string? Image { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        public bool IsForSale { get; set; } = true;
        public bool IsForRent { get; set; } = false;

        [Range(0, 100)]
        public int ConditionPercent { get; set; } = 100;

        [Range(1, 365, ErrorMessage = "Rent duration must be between 1 and 365 days")]
        public int? RentDurationDays { get; set; }
    }
}
