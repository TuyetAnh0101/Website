using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class ProductReview
    {
        [Key]
        public int ProductReviewId { get; set; }

        [Required]
        public int Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public long ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }

    
    public string UserId { get; set; } = default!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // ✅ Thêm dòng này để tránh lỗi
        public string CustomerName { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;
    }
}
