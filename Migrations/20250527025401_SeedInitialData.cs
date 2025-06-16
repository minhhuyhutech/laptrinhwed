using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace caominhhuy.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Kinh Tế" },
                    { 2, "Công Nghệ" },
                    { 3, "Văn Học" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Cuốn sách về nghệ thuật giao tiếp và xây dựng mối quan hệ", "/images/dacnhantam.jpg", "Đắc Nhân Tâm", 120000m },
                    { 2, 2, "Hướng dẫn viết code sạch và dễ bảo trì", "/images/cleancode.jpg", "Clean Code", 150000m },
                    { 3, 3, "Tác phẩm văn học thiếu nhi kinh điển", "/images/demen.jpg", "Dế Mèn Phiêu Lưu Ký", 90000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
