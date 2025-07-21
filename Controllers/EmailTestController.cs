using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SportsStore.Services; // Hoặc namespace chứa IEmailSender của bạn

namespace SportsStore.Controllers
{
    public class EmailTestController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailTestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> SendTest()
        {
            try
            {
                await _emailSender.SendEmailAsync(
                    "tuyetanh131104@gmail.com", // hoặc địa chỉ email khác để test
                    "Test Email từ ASP.NET Core",
                    "<h2>Xin chào!</h2><p>Email này được gửi từ ứng dụng của bạn.</p>");

                return Content("✅ Gửi email thành công!");
            }
            catch (Exception ex)
            {
                return Content("❌ Gửi thất bại: " + ex.Message);
            }
        }
    }
}
