using System;
using System.Linq;

namespace SportsStore.Models
{
    public class EFRentalRepository : IRentalRepository
    {
        private readonly StoreDbContext context;

        public EFRentalRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Rental> Rentals => context.Rentals;

        public void SaveRental(Rental rental)
        {
            if (rental.Id == 0)
            {
                context.Rentals.Add(rental);
            }
            else
            {
                var existing = context.Rentals.Find(rental.Id);
                if (existing != null)
                {
                    existing.BookTitle = rental.BookTitle;
                    existing.StartDate = rental.StartDate;
                    existing.EndDate = rental.EndDate;
                    existing.IsReturned = rental.IsReturned;
                    existing.IsDelivered = rental.IsDelivered;
                    existing.UserId = rental.UserId;
                    existing.ProductId = rental.ProductId;
                }
            }
            context.SaveChanges();
        }

        public void MarkAsDelivered(int rentalId)
        {
            var rental = context.Rentals.Find(rentalId);
            if (rental != null && !rental.IsDelivered)
            {
                rental.IsDelivered = true;
                context.SaveChanges();
            }
        }

        public void MarkAsReturned(int rentalId)
        {
            var rental = context.Rentals.Find(rentalId);
            if (rental != null && !rental.IsReturned)
            {
                rental.IsReturned = true;

                // Tính số ngày trễ hạn nếu có
                var today = DateTime.Today;
                if (today > rental.EndDate)
                {
                    rental.LateReturnDays = (today - rental.EndDate).Days;

                    // Ví dụ tính phí: 10,000 VNĐ/ngày
                    rental.PenaltyFee = rental.LateReturnDays * 10000;

                    // Nếu trễ hơn 7 ngày, đánh dấu nợ xấu
                    if (rental.LateReturnDays > 7)
                    {
                        rental.BadDebtReported = true;
                    }
                }
                else
                {
                    rental.LateReturnDays = 0;
                    rental.PenaltyFee = 0;
                    rental.BadDebtReported = false;
                }

                context.SaveChanges();
            }
        }

        public void DeleteRental(int rentalId)
        {
            var rental = context.Rentals.Find(rentalId);
            if (rental != null && !rental.IsDelivered)
            {
                context.Rentals.Remove(rental);
                context.SaveChanges();
            }
        }
    }
}
