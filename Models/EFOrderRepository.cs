using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));

            // Trừ số lượng sản phẩm khi mua (không áp dụng cho thuê)
            foreach (var line in order.Lines)
            {
                var product = context.Products.FirstOrDefault(p => p.ProductID == line.Product.ProductID);

                if (product != null && line.IsRental == false)
                {
                    product.Quantity -= line.Quantity;

                    if (product.Quantity < 0)
                    {
                        product.Quantity = 0; // Không để số lượng âm
                    }
                }
            }

            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }

            context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            context.RemoveRange(order.Lines);
            context.Orders.Remove(order);
            context.SaveChanges();
        }
    }
}
