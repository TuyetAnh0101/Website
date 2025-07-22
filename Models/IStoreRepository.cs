using System.Linq;

namespace SportsStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }

        void CreateProduct(Product p);
        void SaveProduct(Product p);
        void DeleteProduct(Product p);

        // Lấy sản phẩm theo category
        IQueryable<Product> GetProductsByCategory(int categoryId);

        // Quản lý ảnh phụ cho sản phẩm
        IQueryable<ProductImage> ProductImages { get; }

        void AddProductImage(ProductImage image);
        void DeleteProductImage(long imageId);            
        ProductImage? GetProductImageById(long imageId);

        IQueryable<ProductImage> GetImagesByProductId(long productId);
    }
}
