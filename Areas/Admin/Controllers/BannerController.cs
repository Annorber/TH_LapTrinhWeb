using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TH_LTW_Buoi02.Models;
using TH_LTW_Buoi02.Repositories;

namespace TH_LTW_Buoi02.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BannerController : Controller
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerController(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerRepository.GetAllAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Banner banner)
        {
            if (ModelState.IsValid)
            {
                await _bannerRepository.AddAsync(banner);
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Banner banner)
        {
            if (ModelState.IsValid)
            {
                await _bannerRepository.UpdateAsync(banner);
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bannerRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
