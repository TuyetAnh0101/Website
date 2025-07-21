using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreDbContext _context;

        public HomeController(StoreDbContext context)
        {
            _context = context;
        }

      public async Task<IActionResult> Index(int? categoryId, string? search, string? filterMode, string? priceRange, string? condition, int? rentDuration, int page = 1)
{
    int pageSize = 12;

    // Truy vấn sản phẩm với include Category
    var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

    // Lọc theo categoryId nếu có
    if (categoryId.HasValue)
    {
        productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
    }

    // Lọc theo từ khóa tìm kiếm
    if (!string.IsNullOrEmpty(search))
    {
        productsQuery = productsQuery.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
    }

    // Lọc theo chế độ mua hoặc thuê
    if (filterMode == "buy")
    {
        productsQuery = productsQuery.Where(p => p.IsForSale);
    }
    else if (filterMode == "rent")
    {
        productsQuery = productsQuery.Where(p => p.IsForRent);
    }

    // Lọc theo khoảng giá
    if (!string.IsNullOrEmpty(priceRange))
    {
        productsQuery = priceRange switch
        {
            "lt50" => productsQuery.Where(p => p.Price < 50000),
            "50to200" => productsQuery.Where(p => p.Price >= 50000 && p.Price <= 200000),
            "gt200" => productsQuery.Where(p => p.Price > 200000),
            _ => productsQuery
        };
    }

    // Lọc theo điều kiện sản phẩm
    if (!string.IsNullOrEmpty(condition) && int.TryParse(condition, out int condValue))
    {
        productsQuery = productsQuery.Where(p => p.ConditionPercent >= condValue);
    }

    // Lọc theo thời gian thuê (nếu filterMode là rent)
    if (filterMode == "rent" && rentDuration.HasValue)
    {
        productsQuery = productsQuery.Where(p => p.RentDurationDays >= rentDuration.Value);
    }

    // Lấy tổng số sản phẩm sau lọc để phân trang
    var totalItems = await productsQuery.CountAsync();

    // Lấy sản phẩm theo trang hiện tại
    var items = await productsQuery
        .OrderBy(p => p.ProductID)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // Lấy danh sách gia sư (6 gia sư mới nhất)
    var tutors = await _context.Tutors
        .OrderByDescending(t => t.TutorId)
        .Take(6)
        .ToListAsync();

    // Tạo ViewModel truyền ra View
    var viewModel = new ProductsListViewModel
    {
        Products = items,
        PagingInfo = new PagingInfo
        {
            CurrentPage = page,
            ItemsPerPage = pageSize,
            TotalItems = totalItems
        },
        CurrentCategoryId = categoryId,
        CurrentSearch = search,
        CurrentFilterMode = filterMode,
        CurrentPriceRange = priceRange,
        CurrentCondition = condition,
        CurrentRentDuration = rentDuration,

        Tutors = tutors
    };
    return View(viewModel);
}
    }
}
