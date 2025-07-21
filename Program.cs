using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ===================
// DỊCH VỤ (DI CONFIG)
// ===================

// MVC, Razor, Blazor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Kết nối CSDL chính (sản phẩm, đơn hàng, tài khoản,...)
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

// Cấu hình Identity dùng chung StoreDbContext
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

// Session và Cart
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

// Repository
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IRentalRepository, EFRentalRepository>(); // nếu có thuê sách

// ✅ Cấu hình HttpClient có BaseAddress đúng cho Blazor
builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

// ===================
// ỨNG DỤNG
// ===================
var app = builder.Build();

// ===================
// MIDDLEWARE
// ===================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ===================
// ĐỊNH TUYẾN
// ===================

app.MapControllerRoute("catpage",
    "{category}/Page{productPage:int}",
    new { Controller = "Home", action = "Index" });

app.MapControllerRoute("page", "Page{productPage:int}",
    new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("category", "{category}",
    new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1 });

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();

// Điều hướng fallback cho admin
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

// ===================
// SEED DỮ LIỆU
// ===================
SeedData.EnsurePopulated(app); // Dữ liệu sản phẩm
await IdentitySeedData.EnsurePopulatedAsync(app); // Dữ liệu tài khoản admin

// ===================
// CHẠY ỨNG DỤNG
// ===================
app.Run();
