using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Profile;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Extension;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class ProfileController(UserManager<User> _userManager, AppDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index(string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var favouriteList = await _context.WishLists
                .Where(x => x.UserId == userId)
                .Include(x => x.Advertisement)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Advertisement)
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .ToListAsync();

            if (favouriteList == null || !favouriteList.Any())
            {
                ViewBag.Favourite = new List<WishList>();
            }
            else
            {
                ViewBag.Favourite = favouriteList;
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                Balance = user.Balance,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/photo.jpg" : user.ImageUrl
            };

            return View(model);
        }
        public async Task<IActionResult> FavouriteCar(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var favouriteList = await _context.WishLists
               .Where(x => x.UserId == userId)
               .Include(x => x.Advertisement)
               .ThenInclude(x => x.Brand)
               .Include(x => x.Advertisement)
               .ThenInclude(x => x.Category)
               .Include(x => x.User)
               .ToListAsync();

            if (favouriteList == null || !favouriteList.Any())
            {
                ViewBag.Favourite = new List<WishList>();
            }
            else
            {
                ViewBag.Favourite = favouriteList;
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                Balance = user.Balance,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/photo.jpg" : user.ImageUrl
            };

            return View(model);
        }
        public async Task<IActionResult> MyAdvertisement(string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var myads = await _context.Advertisements.Where(x => x.IsDeleted == false && user.Id == x.UserId )
               .Include(x => x.Brand)
               .Include(x => x.Category)
               .Include(x => x.User)
               .ToListAsync();

            if (myads == null || !myads.Any())
            {
                ViewBag.Ads = new List<WishList>();
            }
            else
            {
                ViewBag.Ads = myads;
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                Balance = user.Balance,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/photo.jpg" : user.ImageUrl
            };

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Medaxil(string? username, int? amount)
        {
            if (!amount.HasValue || amount <= 0)
                return BadRequest("Miqdar düzgün daxil edilməyib.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
                return NotFound("İstifadəçi tapılmadı.");

            // Bakiyeye ekleme işlemi
            user.Balance += amount.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });

        }

        [HttpPost]
        public async Task<IActionResult> Mexaric(string? username, int? amount)
        {
            if (!amount.HasValue || amount <= 0)
                return BadRequest("Miqdar düzgün daxil edilməyib.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
                return NotFound("İstifadəçi tapılmadı.");

            if (user.Balance >= amount)
            {
                user.Balance -= amount.Value;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });

            }
            else
            {
                return BadRequest("Yeterli balans yoxdur.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBalance()
        {
            var username = User.Identity!.Name; 

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound("İstifadəçi tapılmadı.");
            }

            return Ok(new { balance = user.Balance });
        }
        [HttpPost]
        public async Task<IActionResult> GetVIP(string? username, int? adId)
        {
            if (!adId.HasValue)
                return BadRequest("İlan ID'si geçersiz.");

            // Kullanıcıyı bul
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

           
            // İlanı bul
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(ad => ad.Id == adId);
            if (advertisement == null)
            {
                return NotFound("İlan bulunamadı.");
            }

            // İlanın durumunu VIP yap
            advertisement.Status = AdvertisementStatus.VIP;

            // Kullanıcının bakiyesinden 5 birim düşür
            user.Balance -= 5;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarılı işlem sonrası Profile sayfasına yönlendir
            return Json(new { message = "VIP işlemi başarılı!" });
        }







    }
}
