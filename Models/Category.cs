using System.Collections.Generic;

namespace SportsStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool AllowRent { get; set; }

        // Navigation Property
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
