using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class TermsConfirmationModel : PageModel
    {
        private readonly Cart cart;

        public TermsConfirmationModel(Cart cartService)
        {
            cart = cartService;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = "/Order/Checkout";

        public IActionResult OnGet()
        {
            if (!cart.Lines.Any(l => l.IsRental))
            {
                return RedirectToPage("/Cart", new { returnUrl = ReturnUrl });
            }
            return Page();
        }

        public IActionResult OnPostAccept()
        {
            // Chuyển đến trang Checkout sau khi đồng ý
            return Redirect(ReturnUrl);
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("/Cart", new { returnUrl = ReturnUrl });
        }
    }
}
