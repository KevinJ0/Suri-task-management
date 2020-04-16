
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Suri.DTO;
using Suri.Models;

namespace Suri.Controllers
{

    public class EmpleadosController : Controller
    {
        private readonly SuriDbContext _context;
        private readonly UserManager<MyUsers> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<MyUsers> signInManager;

        public EmpleadosController(SignInManager<MyUsers> signInManager, SuriDbContext context, UserManager<MyUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        // GET: Actividades
        public async Task<IActionResult> Index()
        {
            var suriDbContext = _context.Actividades.Include(a => a.Localidad).Include(a => a.MyUser);
            return View(await suriDbContext.ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [Authorize(Roles = "Admin")]
        // GET: Role/Create
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO model)
        {
            if (ModelState.IsValid)
            {

                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Empleados");
                }
                else
                {

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return NotFound();
            }

            var model = new EditRoleDTO
            {
                Id = role.Id,
                RoleName = role.Name


            };
            var p = userManager.Users;
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleDTO dto)
        {
            var role = await roleManager.FindByIdAsync(dto.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {dto.Id} cannot be found";
                return NotFound();
            }
            else
            {

                role.Name = dto.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }


            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }

        }

        [Authorize(Roles = "Admin")]
        // GET: Actividades/Edit/5
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserDTO
            {
                Id = user.Id,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        // POST: Actividades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDTO model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return NotFound();
            }
            else
            {

                user.UserName = model.UserName;
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }


        [Authorize(Roles = "Admin")]
        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }

        }

            private bool ActividadesExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleId = roleId;

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return NotFound();
            }
            var model = new List<UserRoleDTO>();

            foreach (var user in userManager.Users)
            {
                var userRoleDTO = new UserRoleDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleDTO.IsSelected = true;
                }
                else
                {
                    userRoleDTO.IsSelected = false;

                }
                model.Add(userRoleDTO);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string roleId, List<UserRoleDTO> model)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleId = roleId;

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return NotFound();
            }

            foreach (var user in model)
            {
                var MyUser = await userManager.FindByIdAsync(user.UserId);
                IdentityResult result = null;
                if (user.IsSelected && !(await userManager.IsInRoleAsync(MyUser, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(MyUser, role.Name);
                }
                else if (!user.IsSelected && await userManager.IsInRoleAsync(MyUser, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(MyUser, role.Name);

                }
                else
                {
                    continue;
                }

            }
            return RedirectToAction("EditRole", new { Id = roleId });

            //  return View(model);
        }

    }
}
