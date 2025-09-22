using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            StoreDbContext context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Giáo trình cũ", AllowRent = true },
                    new Category { Name = "Dụng cụ học tập", AllowRent = false },
                    new Category { Name = "Balo & Túi xách", AllowRent = false },
                    new Category { Name = "Gia sư", AllowRent = true },

                    // Các danh mục khác
                    new Category { Name = "Sách - Truyện Tranh", AllowRent = true },
                    new Category { Name = "Dụng Cụ Vẽ - VPP", AllowRent = false },
                    new Category { Name = "Bảng Vẽ - Phụ Kiện Số", AllowRent = false },
                    new Category { Name = "Bách Hóa Online", AllowRent = false },
                    new Category { Name = "Quà Tặng - Đồ Chơi", AllowRent = false }
                );
            context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var categories = context.Categories.ToDictionary(c => c.Name, c => c.CategoryId);

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Giáo trình Lập trình C cơ bản",
                        Description = "Sách giáo trình dành cho sinh viên năm nhất CNTT.",
                        CategoryId = categories["Giáo trình cũ"],
                        Price = 45000,
                        RentPrice = 5000,
                        RentDurationDays = 7,
                        Quantity = 20,
                        Image = "laptrinhc.jpg",
                        IsForSale = true,
                        IsForRent = true,
                        ConditionPercent = 90
                    },
                    new Product
                    {
                        Name = "Giáo trình Cấu trúc dữ liệu và giải thuật",
                        Description = "Bản photo còn mới 90%, không gạch xóa.",
                        CategoryId = categories["Giáo trình cũ"],
                        Price = 50000,
                        RentPrice = 7000,
                        RentDurationDays = 7,
                        Quantity = 15,
                        Image = "giaithuat.jpg",
                        IsForSale = true,
                        IsForRent = true,
                        ConditionPercent = 90
                    },
                    new Product
                    {
                        Name = "Máy tính Casio fx-580VN X",
                        Description = "Máy tính còn hoạt động tốt, đầy đủ nút.",
                        CategoryId = categories["Dụng cụ học tập"],
                        Price = 220000,
                        Quantity = 8,
                        Image = "casio580.jpg",
                        IsForSale = true,
                        IsForRent = false,
                        ConditionPercent = 95
                    },
                    new Product
                    {
                        Name = "Bút bi Thiên Long",
                        Description = "Hộp 10 cây bút bi xanh, mới 100%.",
                        CategoryId = categories["Dụng cụ học tập"],
                        Price = 18000,
                        Quantity = 100,
                        Image = "butbi.jpg",
                        IsForSale = true,
                        IsForRent = false,
                        ConditionPercent = 100
                    },
                    new Product
                    {
                        Name = "Balo laptop chống nước",
                        Description = "Balo có ngăn đựng laptop 15 inch, màu đen.",
                        CategoryId = categories["Balo & Túi xách"],
                        Price = 120000,
                        Quantity = 12,
                        Image = "balo-laptop.jpg",
                        IsForSale = true,
                        IsForRent = false,
                        ConditionPercent = 90
                    },
                    new Product
                    {
                        Name = "Balo sinh viên thời trang",
                        Description = "Balo size lớn, nhiều ngăn tiện dụng.",
                        CategoryId = categories["Balo & Túi xách"],
                        Price = 150000,
                        Quantity = 10,
                        Image = "balo-sinhvien.jpg",
                        IsForSale = true,
                        IsForRent = false,
                        ConditionPercent = 95
                    },
                    new Product
                    {
                        Name = "Máy chiếu mini",
                        Description = "Máy chiếu cho các buổi thuyết trình nhỏ.",
                        CategoryId = categories["Dụng cụ học tập"],
                        Price = 50000,
                        Quantity = 3,
                        Image = "maychieu-mini.jpg",
                        IsForSale = true,
                        IsForRent = false,
                        ConditionPercent = 85
                    }
                );
                context.SaveChanges();
            }
             if (!context.Tutors.Any())
            {
                context.Tutors.AddRange(
                    new Tutor
                    {
                        Name = "Nguyễn Văn A",
                        Subject = "Toán",
                        HourlyRate = 150000,
                        Description = "Gia sư Toán cấp 2, cấp 3 với 3 năm kinh nghiệm.",
                        Image = "tutor-a.jpg",
                        PhoneNumber = "0909123456",
                        Email = "vana@gmail.com",
                        Degree = "Cử nhân Sư phạm Toán"
                    },
                    new Tutor
                    {
                        Name = "Trần Thị B",
                        Subject = "Tiếng Anh",
                        HourlyRate = 180000,
                        Description = "Gia sư Anh văn giao tiếp, TOEIC 900+.",
                        Image = "tutor-b.jpg",
                        PhoneNumber = "0912345678",
                        Email = "thib@gmail.com",
                        Degree = "Thạc sĩ Ngôn ngữ Anh"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
