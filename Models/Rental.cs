using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sách.")]
        [StringLength(255)]
        public string BookTitle { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public bool IsReturned { get; set; } = false;
        public bool IsDelivered { get; set; } = false;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        // Số ngày trễ hạn nếu có (0 nếu không trễ)
        public int LateReturnDays { get; set; } = 0;

        // Phí phạt tính theo ngày trễ hạn
        [Column(TypeName = "decimal(8,2)")]
        public decimal PenaltyFee { get; set; } = 0;

        // Cờ đánh dấu đã báo nợ xấu
        public bool BadDebtReported { get; set; } = false;
    }
}
