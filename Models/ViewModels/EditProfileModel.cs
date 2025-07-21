using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
    public class EditProfileModel
    {
        [Display(Name = "Họ tên")]
        public string? FullName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public IFormFile? AvatarFile { get; set; }
    }
}
