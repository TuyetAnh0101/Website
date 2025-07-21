using System;

namespace SportsStore.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public List<string> Months { get; set; } = new();
        public List<int> NewUsersPerMonth { get; set; } = new();
        public List<decimal> RevenuePerMonth { get; set; } = new();
        public List<int> TutorBookingsPerMonth { get; set; } = new();
    }
}
