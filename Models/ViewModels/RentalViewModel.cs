using System;

namespace SportsStore.Models.ViewModels
{
    public class RentalViewModel
    {
        public int Id { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsReturned { get; set; }

        public bool IsDelivered { get; set; }

        // Trạng thái hiển thị tổng hợp
        public string StatusText
        {
            get
            {
                if (IsReturned)
                    return "Đã trả";
                if (!IsDelivered)
                    return "Chờ giao";
                if (IsOverdue)
                    return "Đang thuê (Quá hạn)";
                return "Đang thuê";
            }
        }

        // Số ngày còn lại tính từ ngày giao thực tế
        public int RemainingDays
        {
            get
            {
                if (IsReturned || !IsDelivered)
                    return 0;
                return Math.Max((EndDate - DateTime.Today).Days, 0);
            }
        }

        public bool IsOverdue => !IsReturned && IsDelivered && DateTime.Today > EndDate;

        public string OverdueMessage
        {
            get
            {
                if (IsOverdue)
                    return "Đã quá hạn! Vui lòng trả hàng.";
                if (!IsDelivered)
                    return "Chờ giao hàng.";
                return string.Empty;
            }
        }
    }
}
