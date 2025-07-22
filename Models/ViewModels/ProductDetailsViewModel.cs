using System.Collections.Generic;

namespace SportsStore.Models.ViewModels
{
  public class ProductDetailsViewModel
{
    public Product Product { get; set; }
    public List<ProductImage> ProductImages { get; set; }
    public List<Product> RelatedProducts { get; set; }
    public List<ProductReview> Reviews { get; set; }
}
}
