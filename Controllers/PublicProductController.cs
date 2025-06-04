using caominhhuy.Models;
using caominhhuy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace caominhhuy.Controllers
{
    public class PublicProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PublicProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // ✅ Ai cũng có thể xem danh sách sản phẩm
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // ✅ Ai cũng có thể xem chi tiết sản phẩm
        [AllowAnonymous]
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 🔒 Chỉ Admin mới truy cập được trang thêm sản phẩm
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            await LoadCategoriesAsync();
            return View();
        }

        // 🔒 Xử lý POST: thêm sản phẩm
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                    product.ImageUrl = await SaveImage(imageUrl);

                await _productRepository.AddAsync(product);
                TempData["SuccessMessage"] = "Đã thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        // 🔒 Admin được chỉnh sửa sản phẩm
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        // 🔒 Xử lý POST cập nhật sản phẩm
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");

            if (id != product.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = imageUrl != null
                    ? await SaveImage(imageUrl)
                    : existingProduct.ImageUrl;

                await _productRepository.UpdateAsync(existingProduct);
                TempData["SuccessMessage"] = "Đã cập nhật sản phẩm.";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        // 🔒 Hiển thị form xác nhận xóa
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // 🔒 Xác nhận xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Đã xóa sản phẩm.";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Lưu ảnh vào thư mục wwwroot/images
        private async Task<string> SaveImage(IFormFile image)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

            using var stream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return "/images/" + image.FileName;
        }

        // ✅ Tải danh sách danh mục và gán vào ViewBag
        private async Task LoadCategoriesAsync(int? selectedId = null)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }
    }
}
