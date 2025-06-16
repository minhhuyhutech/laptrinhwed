using caominhhuy.Models;
using caominhhuy.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Cấu hình cache cho session
builder.Services.AddDistributedMemoryCache();

// ✅ Cấu hình session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

// ✅ Inject Repositories
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Kích hoạt session middleware
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Cấu hình routing
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();
app.Run();
