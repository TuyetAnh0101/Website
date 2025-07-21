using System.Linq;

namespace SportsStore.Models
{
    public interface IRentalRepository
    {
        IQueryable<Rental> Rentals { get; }

        void SaveRental(Rental rental);

        void MarkAsDelivered(int rentalId);

        void MarkAsReturned(int rentalId);

        void DeleteRental(int rentalId);
    }
}
