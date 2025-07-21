using System.Linq;

namespace SportsStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }

        void SaveProduct(Product p);
        void CreateProduct(Product p);
        void DeleteProduct(Product p);

        // Bổ sung tiện ích lọc theo category
        IQueryable<Product> GetProductsByCategory(int categoryId);
    }
}
