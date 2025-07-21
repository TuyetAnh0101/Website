namespace SportsStore.Models.ViewModels
{
    public class ProfileModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public List<string> Roles { get; set; } = new();
        
    }
}
