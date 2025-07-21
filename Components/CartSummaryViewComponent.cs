using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        // Constructor dùng Dependency Injection để lấy Cart hiện tại từ dịch vụ
        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService;
        }

        // Gọi khi ViewComponent render, truyền Cart model vào View
        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
