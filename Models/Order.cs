using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [Key]
        [BindNever]
        public int OrderID { get; set; }

        /// <summary>
        /// Dòng sản phẩm trong đơn hàng (thuê hoặc mua).
        /// </summary>
        [BindNever]
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ dòng 1")]
        public string? Line1 { get; set; }

        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thành phố")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tỉnh/thành")]
        public string? State { get; set; }

        public string? Zip { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quốc gia")]
        public string? Country { get; set; }

        public bool GiftWrap { get; set; }

        /// <summary>
        /// Người dùng tạo đơn hàng.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Ngày tạo đơn.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Trạng thái đã giao hàng hay chưa.
        /// </summary>
        [BindNever]
        public bool Shipped { get; set; }

        /// <summary>
        /// Tổng số tiền đơn hàng (bao gồm thuê và mua).
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
    }
}
