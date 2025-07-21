using System;

namespace SportsStore.Models.ViewModels
{
    public class RevenueStats
    {
        public DateTime Date { get; set; }
        public decimal OrderRevenue { get; set; }
        public decimal TutorRevenue { get; set; }
    }
}