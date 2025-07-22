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

        public async Task<IActionResult> Index(int? categoryId, string? search, string? priceRange, string? condition, int? rentDuration, int page = 1)
        {
            int pageSize = 12;

            var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

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

            if (!string.IsNullOrEmpty(condition) && int.TryParse(condition, out int condValue))
            {
                productsQuery = productsQuery.Where(p => p.ConditionPercent >= condValue);
            }

            if (rentDuration.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.RentDurationDays >= rentDuration.Value);
            }

            var totalItems = await productsQuery.CountAsync();
            var items = await productsQuery
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tutors = await _context.Tutors
                .OrderByDescending(t => t.TutorId)
                .Take(6)
                .ToListAsync();

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
                CurrentPriceRange = priceRange,
                CurrentCondition = condition,
                CurrentRentDuration = rentDuration,
                Tutors = tutors
            };

            // Truyền danh mục xuống view để hiển thị dropdown
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CurrentCategoryId = categoryId;

            return View(viewModel);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);

            // Nếu không tìm thấy sản phẩm, quay lại trang chủ
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var images = _context.ProductImages
                .Where(img => img.ProductID == id)
                .ToList();

            var relatedProducts = _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductID != id)
                .Take(8)
                .ToList();

            var reviews = _context.ProductReviews
                .Where(r => r.ProductID == id)
                .OrderByDescending(r => r.Date)
                .ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                ProductImages = images,
                RelatedProducts = relatedProducts,
                Reviews = reviews
            };

            return View(viewModel);
        }
    }
}
