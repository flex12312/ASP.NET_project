using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stepik.Db;
using stepik_asp.Areas.AdminPanel.Models;

namespace stepik_asp.Areas.AdminPanel.Controllers
{
    [Area(Consts.AdminRoleName)]
    [Authorize(Roles = Consts.AdminRoleName)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            var roleViewModels = roles.Select(r => new Role
            {
                Id = Guid.Parse(r.Id),
                Name = r.Name
            }).ToList();

            return View(roleViewModels);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role role)
        {
            if (await _roleManager.FindByNameAsync(role.Name) != null)
            {
                ModelState.AddModelError("", "Такая роль уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role.Name));

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Roles));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }

        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction(nameof(Roles));
        }
    }
}