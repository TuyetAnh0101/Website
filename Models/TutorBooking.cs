using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class TutorBooking
    {
        [Key]
        public int TutorBookingId { get; set; }

        [Required]
        public int TutorId { get; set; }
        public Tutor? Tutor { get; set; }

        // Liên kết với người dùng đã đăng nhập
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Display(Name = "Ngày đặt")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng nhập số giờ thuê.")]
        [Range(1, 24, ErrorMessage = "Thời lượng phải từ 1 đến 24 giờ.")]
        [Display(Name = "Số giờ thuê mỗi ngày")]
        public int DurationHours { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số ngày thuê.")]
        [Range(1, 365, ErrorMessage = "Số ngày thuê phải từ 1 đến 365.")]
        [Display(Name = "Số ngày thuê")]
        public int NumberOfDays { get; set; } = 1;

        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        [Display(Name = "Đã xác nhận")]
        public bool IsConfirmed { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        public DateTime? EndTime => StartTime?.AddHours(DurationHours * NumberOfDays);

        // ✅ Sửa thành thuộc tính lưu được
        [Display(Name = "Tổng giá")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        // --- Thông tin thanh toán ---
        [Display(Name = "Đã thanh toán")]
        public bool IsPaid { get; set; } = false;

        [Display(Name = "Phương thức thanh toán")]
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày thanh toán")]
        public DateTime? PaymentDate { get; set; }

        public void CalculateTotalPrice(decimal hourlyRate)
        {
            TotalPrice = hourlyRate * DurationHours * NumberOfDays;
        }
        public string? PaymentTransactionId { get; set; }
    }
}
