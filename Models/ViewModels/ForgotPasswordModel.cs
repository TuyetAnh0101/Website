using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }
}
}
