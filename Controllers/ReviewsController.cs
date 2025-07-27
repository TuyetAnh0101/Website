using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;


namespace SportsStore.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ReviewsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            StoreDbContext context,
            ILogger<ReviewsController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Hiển thị form đánh giá
        [HttpGet]
        public async Task<IActionResult> Add(long productId)
        {
            _logger.LogInformation("GET: Hiển thị form đánh giá cho sản phẩm ID: {ProductId}", productId);

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Sản phẩm không tồn tại - ID: {ProductId}", productId);
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Người dùng chưa đăng nhập.");
                return RedirectToAction("Login", "Account");
            }

            bool hasReviewed = _context.ProductReviews.Any(r => r.ProductID == productId && r.UserId == userId);
            if (hasReviewed)
            {
                _logger.LogInformation("User ID {UserId} đã đánh giá sản phẩm ID {ProductId}", userId, productId);
                TempData["InfoMessage"] = "Bạn đã đánh giá sản phẩm này.";
                return RedirectToAction("Details", "Home", new { id = productId });
            }

            ViewBag.ProductName = product.Name;
            return View(new ProductReview { ProductID = productId });
        }

        // POST: Xử lý gửi đánh giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductReview review)
        {
            _logger.LogInformation("POST: Gửi đánh giá cho sản phẩm ID: {ProductId}", review.ProductID);

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("Người dùng chưa đăng nhập khi gửi đánh giá.");
                return RedirectToAction("Login", "Account");
            }

            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!_context.Products.Any(p => p.ProductID == review.ProductID))
            {
                _logger.LogWarning("Sản phẩm không tồn tại - ID: {ProductId}", review.ProductID);
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            bool alreadyReviewed = _context.ProductReviews.Any(r => r.ProductID == review.ProductID && r.UserId == userId);
            if (alreadyReviewed)
            {
                _logger.LogInformation("User ID {UserId} đã gửi đánh giá cho sản phẩm ID {ProductId}", userId, review.ProductID);
                TempData["InfoMessage"] = "Bạn đã đánh giá sản phẩm này.";
                return RedirectToAction("Details", "Home", new { id = review.ProductID });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState không hợp lệ khi gửi đánh giá: {Errors}",
                    string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                var product = await _context.Products.FindAsync(review.ProductID);
                ViewBag.ProductName = product?.Name ?? "Sản phẩm";
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin đánh giá.";
                return View(review);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                review.CustomerName = user?.FullName ?? user?.UserName ?? "Người dùng";
                review.Date = DateTime.Now;

                _context.ProductReviews.Add(review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User ID {UserId} đã gửi đánh giá thành công cho sản phẩm ID {ProductId}", userId, review.ProductID);

                TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá sản phẩm!";
                return RedirectToAction("Details", "Home", new { id = review.ProductID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu đánh giá sản phẩm ID {ProductId} bởi User ID {UserId}", review.ProductID, userId);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi gửi đánh giá. Vui lòng thử lại.";
                return View(review);
            }
        }
    }
}
