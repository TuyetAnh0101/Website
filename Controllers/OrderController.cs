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

        public async Task<IActionResult> MyOrders()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Order/MyOrders" });
            }

            var userOrders = orderRepository.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(userOrders);
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

            if (order == null || order.Shipped)
            {
                TempData["Error"] = "Không thể hủy đơn hàng này.";
                return RedirectToAction("MyOrders");
            }

            orderRepository.DeleteOrder(order);
            TempData["Message"] = "Đơn hàng đã được hủy.";
            return RedirectToAction("MyOrders");
        }
    }
}
