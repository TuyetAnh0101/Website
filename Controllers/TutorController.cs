using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    [Authorize]
    public class TutorController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TutorController> _logger;

        public TutorController(StoreDbContext context, UserManager<ApplicationUser> userManager, ILogger<TutorController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Tutor/ListTutors
        [AllowAnonymous]
        public async Task<IActionResult> ListTutors()
        {
            var tutors = await _context.Tutors.ToListAsync();
            _logger.LogInformation("Truy cập danh sách gia sư, tổng: {Count}", tutors.Count);
            return View(tutors);
        }

        // GET: /Tutor/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor == null)
            {
                _logger.LogWarning("Không tìm thấy gia sư Id={Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Xem chi tiết gia sư Id={Id}, tên={Name}", tutor.TutorId, tutor.Name);
            return View(tutor);
        }

        // POST: /Tutor/Book
        // Người dùng nhập form thuê gia sư → validate → hiện trang ConfirmBooking để xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(
            int TutorId,
            int DurationHours,
            int NumberOfDays,
            DateTime StartTime,
            string? Notes)
        {
            _logger.LogInformation("User {User} đặt thuê gia sư Id={TutorId}, {DurationHours} giờ/ngày, {NumberOfDays} ngày, bắt đầu {StartTime}", 
                User.Identity?.Name, TutorId, DurationHours, NumberOfDays, StartTime);

            var tutor = await _context.Tutors.FindAsync(TutorId);
            if (tutor == null)
            {
                _logger.LogWarning("Gia sư Id={TutorId} không tồn tại", TutorId);
                return NotFound("Không tìm thấy gia sư.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User chưa đăng nhập hoặc không tồn tại");
                return Unauthorized();
            }

            // Validate dữ liệu nhập
            if (DurationHours < 1 || DurationHours > 24)
                ModelState.AddModelError(nameof(DurationHours), "Số giờ thuê phải từ 1 đến 24.");

            if (NumberOfDays < 1 || NumberOfDays > 365)
                ModelState.AddModelError(nameof(NumberOfDays), "Số ngày thuê phải từ 1 đến 365.");

            if (StartTime.Date < DateTime.Now.Date)
                ModelState.AddModelError(nameof(StartTime), "Ngày bắt đầu phải từ hôm nay trở đi.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu nhập không hợp lệ: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Details", tutor);
            }

            var booking = new TutorBooking
            {
                TutorId = TutorId,
                Tutor = tutor,
                CustomerName = user.FullName ?? user.UserName ?? "Khách",
                CustomerPhone = user.PhoneNumber ?? "",
                BookingDate = DateTime.Now,
                DurationHours = DurationHours,
                NumberOfDays = NumberOfDays,
                StartTime = StartTime,
                Notes = Notes,
                IsConfirmed = false,
                IsPaid = false
            };

            _logger.LogInformation("Tạo đối tượng booking: {@Booking}", booking);

            // Trả về view xác nhận đặt lịch
            return View("ConfirmBooking", booking);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ConfirmBooking(TutorBooking booking)
{
    _logger.LogInformation("User {User} xác nhận booking gia sư Id={TutorId} bắt đầu {StartTime}", 
        User.Identity?.Name, booking.TutorId, booking.StartTime);

    try
    {
        var tutor = await _context.Tutors.FindAsync(booking.TutorId);
        if (tutor == null)
        {
            _logger.LogWarning("Không tìm thấy gia sư Id={TutorId} khi xác nhận booking", booking.TutorId);
            return NotFound("Không tìm thấy gia sư.");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User chưa đăng nhập hoặc không tồn tại khi xác nhận booking");
            return Unauthorized();
        }

        booking.CustomerName = user.FullName ?? user.UserName ?? "Khách";
        booking.CustomerPhone = user.PhoneNumber ?? "";
        booking.BookingDate = DateTime.Now;
        booking.IsConfirmed = true;
        booking.IsPaid = false;
        booking.Tutor = null; // tránh tracking thừa
        booking.UserId = user.Id;

        _context.TutorBookings.Add(booking);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Booking Id={BookingId} đã được lưu, chuyển sang trang thanh toán", booking.TutorBookingId);

        return RedirectToAction("Payment", new { bookingId = booking.TutorBookingId });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Lỗi khi xác nhận booking cho gia sư Id={TutorId}", booking.TutorId);
        TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình xác nhận đặt lịch. Vui lòng thử lại.";
        return RedirectToAction("Details", new { id = booking.TutorId });
    }
}

        // GET: /Tutor/Payment/5
        [HttpGet]
        public async Task<IActionResult> Payment(int bookingId)
        {
            _logger.LogInformation("User {User} truy cập trang thanh toán booking Id={BookingId}", User.Identity?.Name, bookingId);

            var booking = await _context.TutorBookings
                .Include(b => b.Tutor)
                .FirstOrDefaultAsync(b => b.TutorBookingId == bookingId);

            if (booking == null)
            {
                _logger.LogWarning("Booking Id={BookingId} không tồn tại khi truy cập thanh toán", bookingId);
                return NotFound();
            }

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int bookingId, string paymentMethod)
        {
            _logger.LogInformation("User {User} tiến hành thanh toán booking Id={BookingId} với phương thức {PaymentMethod}",
                User.Identity?.Name, bookingId, paymentMethod);

            var booking = await _context.TutorBookings
                .Include(b => b.Tutor)
                .FirstOrDefaultAsync(b => b.TutorBookingId == bookingId);

            if (booking == null)
            {
                _logger.LogWarning("Booking Id={BookingId} không tồn tại khi thanh toán", bookingId);
                return NotFound();
            }

            // Nếu đã thanh toán thì thông báo
            if (booking.IsPaid)
            {
                TempData["InfoMessage"] = "Đơn đặt lịch này đã được thanh toán trước đó.";
                return RedirectToAction(nameof(MyBookings));
            }

            // Nếu không chọn phương thức thanh toán
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                TempData["ErrorMessage"] = "Vui lòng chọn phương thức thanh toán.";
                return RedirectToAction(nameof(Payment), new { id = bookingId });
            }

            // ✅ Tính tổng giá dựa vào Tutor.HourlyRate
            if (booking.Tutor != null)
            {
                booking.CalculateTotalPrice(booking.Tutor.HourlyRate);
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể tính tổng giá vì thông tin gia sư không tồn tại.";
                return RedirectToAction(nameof(Payment), new { id = bookingId });
            }

            booking.IsPaid = true;
            booking.PaymentMethod = paymentMethod;
            booking.PaymentDate = DateTime.Now;
            booking.PaymentTransactionId = Guid.NewGuid().ToString();

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Thanh toán thành công! Tổng số tiền là {booking.TotalPrice:N0} VNĐ.";
            _logger.LogInformation("Booking Id={BookingId} đã thanh toán thành công qua {PaymentMethod}, Tổng tiền: {TotalPrice}",
                bookingId, paymentMethod, booking.TotalPrice);

            return RedirectToAction(nameof(MyBookings));
        }

        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogWarning("User chưa đăng nhập nhưng truy cập MyBookings");
                return Unauthorized(); // Hoặc redirect về trang login
            }

            var bookings = await _context.TutorBookings
                .Include(b => b.Tutor)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            _logger.LogInformation("User {User} xem danh sách booking, tổng cộng: {Count}", 
                user.UserName, bookings.Count);

            return View(bookings);
        }
    }
}
