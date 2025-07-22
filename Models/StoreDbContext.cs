using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<TutorBooking> TutorBookings { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductReview> ProductReviews { get; set; }
    }
}
