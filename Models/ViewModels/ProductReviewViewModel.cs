using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
    public class ProductReviewViewModel
    {
        // Thông tin sản phẩm
        public int ProductID { get; set; }

        public string ProductName { get; set; } = string.Empty;

        // Thông tin người đánh giá
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung đánh giá không được để trống.")]
        [MaxLength(1000, ErrorMessage = "Nội dung đánh giá không được vượt quá 1000 ký tự.")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn số sao đánh giá.")]
        [Range(1, 5, ErrorMessage = "Số sao phải nằm trong khoảng từ 1 đến 5.")]
        public int Rating { get; set; }

        public string UserId { get; set; } = string.Empty;

        // Danh sách đánh giá hiện có
        public List<ProductReview>? ExistingReviews { get; set; }
    }
}
