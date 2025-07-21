using Microsoft.AspNetCore.Identity;

namespace SportsStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }

        public ICollection<Rental>? Rentals { get; set; }
        public ICollection<TutorBooking>? TutorBookings { get; set; }
        public bool IsAdmin { get; set; }
    }
}
