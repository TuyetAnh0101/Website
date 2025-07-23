using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Localization;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreRepository repository;

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl, bool isRental = false, int rentalDays = 0)
        {
            Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product == null)
            {
                ModelState.AddModelError("", "Không tìm thấy sản phẩm.");
                return Page();
            }

            if (isRental && product.IsForRent)
            {
                rentalDays = rentalDays > 0 ? rentalDays : (product.RentDurationDays ?? 1);
                Cart.AddItem(product, 1, true, rentalDays);
            }
            else if (!isRental && product.IsForSale)
            {
                Cart.AddItem(product, 1);
            }

            return RedirectToPage(new { returnUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl, bool isRental = false)
        {
            var line = Cart.Lines.FirstOrDefault(cl =>
                cl.Product.ProductID == productId && cl.IsRental == isRental);

            if (line != null)
            {
                Cart.RemoveLine(line.Product, isRental);
            }

            return RedirectToPage(new { returnUrl });
        }

        public IActionResult OnPostProceedToCheckout(string returnUrl)
        {
            if (Cart.Lines.Any(l => l.IsRental))
            {
                // Chuyển sang trang điều khoản nếu có sản phẩm thuê
                return RedirectToPage("/TermsConfirmation", new { returnUrl = "/Order/Checkout" });
            }
            else
            {
                // Mua bình thường
                return RedirectToAction("Checkout", "Order");
            }
        }
    }
}
