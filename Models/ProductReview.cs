using System;

namespace SportsStore.Models
{
public class ProductReview
{
    public int ProductReviewId { get; set; }
    public int ProductID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;

    // Navigation
    public Product? Product { get; set; }
}
}
