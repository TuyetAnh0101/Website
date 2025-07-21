namespace SportsStore.Models
{
    public class CartLineSession
    {
        public long ProductID { get; set; }
        public int Quantity { get; set; }
        public bool IsRental { get; set; }
        public int RentalDays { get; set; }
    }
}
