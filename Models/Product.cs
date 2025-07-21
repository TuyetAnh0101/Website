using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Product
    {
        public long? ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal? RentPrice { get; set; }

        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; } = null!;

        public string? Image { get; set; }

        public int Quantity { get; set; }
        public bool IsForSale { get; set; } = true;
        public bool IsForRent { get; set; } = false;
        public int ConditionPercent { get; set; } = 100;
        public int? RentDurationDays { get; set; }
    }
}
