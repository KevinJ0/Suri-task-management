
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
    public class AccountController : Controller
    {
        private readonly SuriDbContext _context;
        private readonly UserManager<MyUsers> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<MyUsers> signInManager;

     

        public AccountController(SignInManager<MyUsers> signInManager, SuriDbContext context, UserManager<MyUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {

            if (signInManager.IsSignedIn(User))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }


            }
            ViewBag.ErrorInicioSeccion = "Usuario o contraseña incorrectos";

            return View(dto);
        }
        [AllowAnonymous] // when you have a global authorize on the class creater
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MyUsersDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = new MyUsers { UserName = dto.UserName };
                var result = await userManager.CreateAsync(user, dto.Password);

                if (result.Succeeded)
                {

                    return RedirectToAction("ListUsers", "Empleados");
                }
                else {

                    ModelState.AddModelError("ErrorExist","Este usuario ya se encuentra registrado");
                }


            }
            else
            {

                ModelState.AddModelError("ErrorExist", "Este Modelo no es valido");


            }

            return View(dto);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
