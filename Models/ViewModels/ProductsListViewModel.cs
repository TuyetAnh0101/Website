using System.Collections.Generic;
using SportsStore.Models;

namespace SportsStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        // Từ khóa tìm kiếm
        public string? CurrentSearch { get; set; }

        // Mua hoặc Thuê (buy / rent)
        public string? CurrentFilterMode { get; set; }

        // Khoảng giá đã chọn (lt50, 50to200, gt200)
        public string? CurrentPriceRange { get; set; }

        // Độ mới (%): 70, 80, 90, 100
        public string? CurrentCondition { get; set; }

        // Thời hạn thuê (ngày)
        public int? CurrentRentDuration { get; set; }

        // Danh sách sản phẩm kết quả
        public IEnumerable<Product>? Products { get; set; }

        // Phân trang
        public PagingInfo? PagingInfo { get; set; }

        // Danh mục đang lọc (nếu có)
        public int? CurrentCategoryId { get; set; }

        // **Thêm danh sách gia sư**
        public IEnumerable<Tutor>? Tutors { get; set; }
        // Thêm vào cuối class
        public IEnumerable<Category>? Categories { get; set; }

    }
}
