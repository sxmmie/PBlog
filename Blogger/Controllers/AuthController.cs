using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

            if (!result.Succeeded)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if(isAdmin)
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new IdentityUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, "password");
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}