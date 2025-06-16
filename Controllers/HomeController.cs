using System.Diagnostics;
using caominhhuy.Models;
using caominhhuy.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace caominhhuy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        // ✅ Gộp cả 2 dependency vào 1 constructor
        public HomeController(IProductRepository productRepository, ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
