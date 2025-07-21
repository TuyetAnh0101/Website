using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetRevenueStats(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
            {
                return BadRequest("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }

            var startDate = fromDate?.Date ?? DateTime.MinValue.Date;
            var endDate = toDate?.Date ?? DateTime.MaxValue.Date;

         // 🧾 Đơn hàng
var orderData = await _context.Orders
    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
    .Include(o => o.Lines)
        .ThenInclude(l => l.Product)
    .ToListAsync();

var groupedOrders = orderData
    .GroupBy(o => o.OrderDate.Date)
    .ToDictionary(
        g => g.Key,
        g => g.Sum(o => o.Lines.Sum(l => l.Quantity * l.Product.Price))
    );

            // 🎓 Thuê gia sư
            var tutorData = await _context.TutorBookings
                .Where(tb => tb.BookingDate >= startDate && tb.BookingDate <= endDate && tb.IsPaid)
                .Select(tb => new
                {
                    Date = tb.BookingDate.Date,
                    Total = tb.TotalPrice  
                })
                .ToListAsync();

            var groupedTutor = tutorData
                .GroupBy(x => x.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Total)
                );

            // 📅 Tổng hợp ngày
            var allDates = groupedOrders.Keys
                .Union(groupedTutor.Keys)
                .Distinct()
                .OrderBy(d => d);

            // 📊 Dữ liệu cuối cùng
            var result = allDates.Select(date => new RevenueStats
            {
                Date = date,
                OrderRevenue = groupedOrders.ContainsKey(date) ? groupedOrders[date] : 0m,
                TutorRevenue = groupedTutor.ContainsKey(date) ? groupedTutor[date] : 0m
            }).ToList();

            return Ok(result);
        }
    }
}
