using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new();

        public virtual void AddItem(Product product, int quantity, bool isRental = false, int rentalDays = 0)
        {
            if (product == null || quantity <= 0) return;

            var line = Lines.FirstOrDefault(p => p.Product.ProductID == product.ProductID && p.IsRental == isRental);

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    IsRental = isRental,
                    RentalDays = isRental ? rentalDays : 0
                });
            }
            else
            {
                line.Quantity += quantity;
                if (isRental && rentalDays > 0)
                {
                    line.RentalDays = rentalDays;
                }
            }
        }

        public virtual void RemoveLine(Product product, bool isRental = false) =>
            Lines.RemoveAll(l => l.Product.ProductID == product.ProductID && l.IsRental == isRental);

        public decimal ComputeTotalValue() =>
            Lines.Sum(l => l.LineTotal);

        public virtual void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
        public bool IsRental { get; set; } = false;
        public int RentalDays { get; set; } = 0;

        public decimal LineTotal =>
            IsRental
                ? (Product.RentPrice ?? 0) * System.Math.Max(RentalDays, 1) * Quantity
                : Product.Price * Quantity;
    }
}
