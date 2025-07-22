using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        // Liên kết với sản phẩm
        [ForeignKey("Product")]
        public long ProductID { get; set; }

        public Product Product { get; set; } = null!;
    }
}
