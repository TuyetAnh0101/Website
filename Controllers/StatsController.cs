using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SportsStore.Controllers
{
    [Route("admin/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public StatsController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue")]
        public IActionResult GetRevenueStats(DateTime? fromDate, DateTime? toDate)
        {
            var startDate = fromDate?.Date ?? DateTime.MinValue;
            var endDate = toDate?.Date ?? DateTime.MaxValue;

            var orderData = _context.Orders
                .Include(o => o.Lines)
                    .ThenInclude(cl => cl.Product)
                .Where(o => o.OrderDate.Date >= startDate && o.OrderDate.Date <= endDate)
                .Select(o => new
                {
                    Date = o.OrderDate.Date,
                    Total = o.Lines.Sum(cl => cl.Quantity * cl.Product.Price)
                })
                .GroupBy(x => x.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Total)
                );

            var tutorData = _context.TutorBookings
                .Where(tb => tb.BookingDate.Date >= startDate && tb.BookingDate.Date <= endDate)
                .GroupBy(tb => tb.BookingDate.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(tb => tb.TotalPrice ?? 0)
                );

            var allDates = orderData.Keys
                .Union(tutorData.Keys)
                .Distinct()
                .OrderBy(d => d);

            var result = allDates.Select(date => new RevenueStats
            {
                Date = date,
                OrderRevenue = orderData.ContainsKey(date) ? orderData[date] : 0,
                TutorRevenue = tutorData.ContainsKey(date) ? tutorData[date] : 0
            }).ToList();

            return Ok(result);
        }
    }
}
