using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BarangayCMS.Entities;
using BarangayCMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarangayCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 1. GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Paghati sa FullName para sa ViewModel (fallback kung walang space)
                string firstName = user.FullName;
                string lastName = string.Empty;
                int lastSpaceIndex = user.FullName.LastIndexOf(' ');

                if (lastSpaceIndex >= 0)
                {
                    firstName = user.FullName.Substring(0, lastSpaceIndex);
                    lastName = user.FullName.Substring(lastSpaceIndex + 1);
                }

                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = user.Email ?? "",
                    Username = user.UserName ?? "",
                    Role = roles.FirstOrDefault() ?? user.Role ?? "No Role"
                });
            }

            return View(userList);
        }

        // 2. GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        // 3. POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName = $"{model.FirstName} {model.LastName}".Trim(), // Pinagsama sa bagong FullName property
                    Role = model.Role ?? "Staff",
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                // Gamitin ang default password kung walang nilagay para iwas crash
                string password = string.IsNullOrEmpty(model.Password) ? "Barangay123!" : model.Password;
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Siguraduhing gawa muna ang Role bago i-assign
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(model.Role));
                        }
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    TempData["SuccessMessage"] = "Account successfully registered!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // 4. GET: Admin/Users/Roles
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // 5. GET: Admin/Users/Permissions
        public IActionResult Permissions()
        {
            return View();
        }
    }
}