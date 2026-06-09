using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TH_LTW_Buoi02.Models;
using TH_LTW_Buoi02.Repositories;

namespace TH_LTW_Buoi02.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBannerRepository _bannerRepository;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository, IBannerRepository bannerRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _bannerRepository = bannerRepository;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var products = await _productRepository.GetAllAsync();
        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            ViewBag.SelectedCategoryId = categoryId.Value;
        }
        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        ViewBag.Banners = await _bannerRepository.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Display(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
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
