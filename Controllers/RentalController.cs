using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly IRentalRepository rentalRepo;
        private readonly IStoreRepository storeRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public RentalController(IRentalRepository rentalRepo, IStoreRepository storeRepo, UserManager<ApplicationUser> userMgr)
        {
            this.rentalRepo = rentalRepo;
            this.storeRepo = storeRepo;
            this.userManager = userMgr;
        }

        public async Task<IActionResult> MyRentals()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var rentals = rentalRepo.Rentals
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.StartDate)
                .Select(r => new RentalViewModel
                {
                    Id = r.Id,
                    BookTitle = r.BookTitle,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    IsReturned = r.IsReturned,
                    IsDelivered = r.IsDelivered
                })
                .ToList();

            return View(rentals);
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int productId, int durationDays)
        {
            if (durationDays <= 0)
            {
                TempData["Error"] = "Số ngày thuê phải lớn hơn 0.";
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var product = storeRepo.Products.FirstOrDefault(p => p.ProductID == productId && p.IsForRent);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại hoặc không cho thuê.";
                return RedirectToAction("Index", "Home");
            }

            var rental = new Rental
            {
                BookTitle = product.Name,
                StartDate = DateTime.Today, // Tạm thời lưu ngày tạo đơn
                EndDate = DateTime.Today.AddDays(durationDays),
                IsReturned = false,
                IsDelivered = false,
                UserId = user.Id,
                ProductId = product.ProductID
            };

            rentalRepo.SaveRental(rental);
            TempData["Success"] = $"Đã tạo đơn thuê '{product.Name}' chờ giao.";
            return RedirectToAction("MyRentals");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ConfirmDelivery(int rentalId)
        {
            var rental = rentalRepo.Rentals.FirstOrDefault(r => r.Id == rentalId);
            if (rental != null && !rental.IsDelivered)
            {
                var duration = (rental.EndDate - rental.StartDate).Days;
                rental.IsDelivered = true;
                rental.StartDate = DateTime.Today;
                rental.EndDate = DateTime.Today.AddDays(duration);
                rentalRepo.SaveRental(rental);
                TempData["Message"] = "Đã xác nhận giao hàng.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy đơn thuê hoặc đã giao trước đó.";
            }
            return RedirectToAction("MyRentals");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmReturn(int rentalId)
        {
            var user = await userManager.GetUserAsync(User);
            var rental = rentalRepo.Rentals.FirstOrDefault(r => r.Id == rentalId && r.UserId == user.Id);

            if (rental != null && rental.IsDelivered && !rental.IsReturned)
            {
                rental.IsReturned = true;
                rentalRepo.SaveRental(rental);
                TempData["Message"] = "Bạn đã xác nhận trả hàng.";
            }
            else
            {
                TempData["Error"] = "Không thể xác nhận trả cho đơn không hợp lệ.";
            }

            return RedirectToAction("MyRentals");
        }

        [HttpPost]
        public async Task<IActionResult> CancelRental(int rentalId)
        {
            var user = await userManager.GetUserAsync(User);
            var rental = rentalRepo.Rentals.FirstOrDefault(r => r.Id == rentalId && r.UserId == user.Id);

            if (rental != null && !rental.IsDelivered)
            {
                rentalRepo.DeleteRental(rentalId);
                TempData["Message"] = "Đơn thuê đã được hủy.";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn đã giao hoặc không tồn tại.";
            }

            return RedirectToAction("MyRentals");
        }
    }
}
