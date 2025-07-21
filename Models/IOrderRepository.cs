namespace SportsStore.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        // Lưu đơn hàng mới
        void SaveOrder(Order order);

        // Hủy đơn hàng (chỉ áp dụng khi chưa giao)
        void DeleteOrder(Order order);
    }
}
