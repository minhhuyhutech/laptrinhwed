using caominhhuy.Models;
using caominhhuy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace caominhhuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await _productRepository.AddAsync(product);
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công.";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            if (id != product.Id) return BadRequest();

            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = imageUrl != null ? await SaveImage(imageUrl) : existingProduct.ImageUrl;

                await _productRepository.UpdateAsync(existingProduct);
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Sản phẩm đã được xóa.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null) return null;

            var fileName = Path.GetFileName(image.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using var stream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return "/images/" + fileName;
        }
    }
}