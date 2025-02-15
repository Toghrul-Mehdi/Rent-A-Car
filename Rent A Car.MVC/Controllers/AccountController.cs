using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Rent_A_Car.BL.DTOs.Auth;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class AccountController(UserManager<User> _usermanager, SignInManager<User> _signinmanager) : Controller
    {
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return View();



            if (await _usermanager.Users.AnyAsync(x => x.Email == dto.Email))
            {
                ModelState.AddModelError("", "Email movcuddur");
                return View();
            }

            User user = new User
            {
                Fullname = dto.Username,
                Email = dto.Email,
                UserName = dto.Username,
                ImageUrl = "photo.jpg",
                PasswordHash = dto.Password
            };

            var result = await _usermanager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginDTO dto, string? returnUrl = "/")
        {
            if (!ModelState.IsValid) return View();

            User? user = dto.UsernameOrEmail.Contains("@")
                ? await _usermanager.FindByEmailAsync(dto.UsernameOrEmail)
                : await _usermanager.FindByNameAsync(dto.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found. Please check your username or email.");
                return View();
            }

            var result = await _signinmanager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect password. Please try again.");
                return View();
            }


            /*var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),  
        new Claim("Fullname", user.Fullname ?? ""),  
        new Claim("ImageUrl", user.ImageUrl ?? "default.png")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);*/

            
            if (await _usermanager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



    }
}
