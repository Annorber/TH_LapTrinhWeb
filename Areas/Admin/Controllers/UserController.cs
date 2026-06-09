using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TH_LTW_Buoi02.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TH_LTW_Buoi02.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }
            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            
            ViewBag.Roles = new SelectList(roles);
            ViewBag.CurrentRole = currentRoles.FirstOrDefault() ?? SD.Role_Customer;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string newRole)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            
            if (!string.IsNullOrEmpty(newRole))
            {
                await _userManager.AddToRoleAsync(user, newRole);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
