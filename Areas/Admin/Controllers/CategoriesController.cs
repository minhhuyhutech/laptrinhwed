using caominhhuy.Models;
using caominhhuy.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caominhhuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoriesController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // GET: Admin/Categories/Display/5
        public async Task<IActionResult> Display(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Admin/Categories/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Update/5
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Category category)
        {
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
            if (categoryToDelete != null)
            {
                await _categoryRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
