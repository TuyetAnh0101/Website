using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using System;
using System.Security.Claims;


namespace SportsStore.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(StoreDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Reviews/Add?id=productId&userId=...&customerName=...
        public IActionResult Add(int id, string userId, string customerName)
        {
            _logger.LogInformation("Rendering review form for product ID: {ProductId}, User: {UserId}", id, userId);

            var review = new ProductReview
            {
                ProductID = id,
                UserId = userId,
                CustomerName = customerName
            };

            return View(review);
        }
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Add(ProductReview review)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (string.IsNullOrEmpty(userId))
    {
        _logger.LogWarning("User chưa đăng nhập, không thể đánh giá");
        return RedirectToAction("Login", "Account");
    }

    review.UserId = userId;  // Gán UserId từ claim đăng nhập

    if (!ModelState.IsValid)
    {
        return View(review);
    }

    review.Date = DateTime.Now;
    _context.ProductReviews.Add(review);
    _context.SaveChanges();

    return RedirectToAction("Details", "Home", new { id = review.ProductID });
}

    }
}
