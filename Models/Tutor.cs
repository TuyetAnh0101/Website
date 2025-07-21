using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Range(1, 1000000)]
        public decimal HourlyRate { get; set; }

        public string? Description { get; set; }
        public string? Image { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Degree { get; set; } // Cử nhân, Thạc sĩ,...
        public string? Experience { get; set; }
         public virtual ICollection<TutorBooking> TutorBookings { get; set; } = new List<TutorBooking>();

    }
}
