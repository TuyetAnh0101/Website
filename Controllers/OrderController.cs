using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IRentalRepository rentalRepository;
        private readonly Cart cart;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderController(IOrderRepository orderRepo,
                               IRentalRepository rentalRepo,
                               Cart cartService,
                               UserManager<ApplicationUser> userMgr)
        {
            orderRepository = orderRepo;
            rentalRepository = rentalRepo;
            cart = cartService;
            userManager = userMgr;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/Checkout" });
            }

            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                return View(order);
            }

            var purchaseLines = cart.Lines.Where(l => !l.IsRental).ToList();
            var rentalLines = cart.Lines.Where(l => l.IsRental).ToList();

            if (purchaseLines.Any())
            {
                order.UserId = user.Id;
                order.Lines = purchaseLines;

                // ✅ Tính tổng tiền và gán vào cột TotalAmount
                order.TotalAmount = purchaseLines.Sum(line => line.LineTotal);

                // ✅ Gán trạng thái mặc định ban đầu
                order.Status = OrderStatus.ChoXacNhan;

                // Lưu đơn hàng mua
                orderRepository.SaveOrder(order);
            }

            if (rentalLines.Any())
            {
                foreach (var line in rentalLines)
                {
                    var rental = new Rental
                    {
                        UserId = user.Id,
                        BookTitle = line.Product.Name,
                        StartDate = System.DateTime.Today,
                        EndDate = System.DateTime.Today.AddDays(line.RentalDays),
                        IsReturned = false
                    };
                    rentalRepository.SaveRental(rental);
                }
            }

            cart.Clear();
            return RedirectToPage("/Completed");
        }

        public async Task<IActionResult> MyOrders(OrderStatus? status = null)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/MyOrders" });
            }

            var orders = orderRepository.Orders
                .Where(o => o.UserId == user.Id);

            if (status != null)
            {
                orders = orders.Where(o => o.Status == status);
            }

            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Order/Details/{id}" });
            }

            var order = orderRepository.Orders
                .FirstOrDefault(o => o.OrderID == id && o.UserId == user.Id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = orderRepository.Orders
                .FirstOrDefault(o => o.OrderID == id && o.UserId == user.Id);

            if (order == null || order.Status != OrderStatus.ChoXacNhan)
            {
                TempData["Error"] = "Không thể hủy đơn hàng này.";
                return RedirectToAction("MyOrders");
            }

            order.Status = OrderStatus.DaHuy;
            orderRepository.SaveOrder(order);

            TempData["Message"] = "Đơn hàng đã được chuyển sang trạng thái 'Đã hủy'.";
            return RedirectToAction("MyOrders");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus newStatus)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var order = orderRepository.Orders
                .FirstOrDefault(o => o.OrderID == id && o.UserId == user.Id);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("MyOrders");
            }

            order.Status = newStatus;
            orderRepository.SaveOrder(order);

            TempData["Message"] = $"Đã cập nhật trạng thái đơn hàng #{id} thành công.";
            return RedirectToAction("MyOrders");
        }
    }
}
