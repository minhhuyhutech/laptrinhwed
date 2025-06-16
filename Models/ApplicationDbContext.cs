using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace caominhhuy.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu Category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Kinh Tế" },
                new Category { Id = 2, Name = "Công Nghệ" },
                new Category { Id = 3, Name = "Văn Học" }
            );

            // Seed dữ liệu Product (các cuốn sách)
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Đắc Nhân Tâm",
                    Price = 120000m,
                    Description = "Cuốn sách về nghệ thuật giao tiếp và xây dựng mối quan hệ",
                    ImageUrl = "/images/dacnhantam.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Clean Code",
                    Price = 150000m,
                    Description = "Hướng dẫn viết code sạch và dễ bảo trì",
                    ImageUrl = "/images/cleancode.jpg",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Dế Mèn Phiêu Lưu Ký",
                    Price = 90000m,
                    Description = "Tác phẩm văn học thiếu nhi kinh điển",
                    ImageUrl = "/images/demen.jpg",
                    CategoryId = 3
                }
            );
        }
    }
}
