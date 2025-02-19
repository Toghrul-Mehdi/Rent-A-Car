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
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Rent_A_Car.BL.Helpers;

namespace Rent_A_Car.MVC.Controllers
{
    public class AccountController(UserManager<User> _usermanager, SignInManager<User> _signinmanager, IOptions<SmtpOptions> options) : Controller
    {
        readonly SmtpOptions _smtp = options.Value;
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

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("E-posta tapilmadi.");
            }

            var token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                action: "ResetPassword",
                controller: "Account",
                values: new { token, email = user.Email },
                protocol: "https");

            SmtpClient smtp = new SmtpClient
            {
                Host = _smtp.Host,
                Port = _smtp.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtp.Username, _smtp.Password)
            };

            MailMessage msg = new MailMessage
            {
                From = new MailAddress(_smtp.Username, "Togrul Mehdiyev CodeAcademy"),
                Subject = "Reset Password",
                Body = $"<p>Şifrenizi sıfırlamak için <a href='{resetLink}'>bu bağlantıya</a> tıklayın.</p>",
                IsBodyHtml = true
            };
            msg.To.Add(email);

            smtp.Send(msg);

            return Ok("Emaile gonderildi");
        }

        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Connection");
            }

            return View(new ResetPasswordDTO { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _usermanager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                return BadRequest("Istifadeci tapilmadi");
            }

            var resetPassResult = await _usermanager.ResetPasswordAsync(user, vm.Token, vm.NewPassword);
            if (resetPassResult.Succeeded)
            {
                return Ok("Hershey ugurla tamamlandi");
            }

            foreach (var error in resetPassResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }



    }
}
