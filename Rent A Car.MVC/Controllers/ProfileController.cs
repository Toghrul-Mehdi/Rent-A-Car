using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rent_A_Car.BL.DTOs.Advertisement;
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

            var orders = await _context.Bookings
                .Include(x => x.Advertisement)
                    .ThenInclude(x => x.Brand)
                .Include(x => x.Advertisement)
                    .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.UserID == userId) 
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync();

            
            ViewBag.Orders = orders;

            var renters = await _context.Bookings
                .Include(x => x.Advertisement)
                    .ThenInclude(x => x.Brand)
                .Include(x => x.Advertisement)
                    .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.Advertisement.UserId == userId)
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync();


            ViewBag.Renters = renters;


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
            ViewBag.AdvCount = await _context.Advertisements.Include(x => x.User).Where(x => x.UserId == userId && x.IsConfirmed == true).CountAsync();
            ViewBag.OrderCount = await _context.Bookings.Include(x => x.User).Where(x => x.UserID == userId).CountAsync();
            ViewBag.FavCount = await _context.WishLists
                .Where(x => x.UserId == userId)
                .Include(x => x.Advertisement)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Advertisement)
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.Advertisement.IsDeleted == false && x.Advertisement.IsConfirmed == true)
                .CountAsync();
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
   .Where(x => x.Advertisement.IsDeleted == false && x.Advertisement.IsConfirmed == true)
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

            var myads = await _context.Advertisements.Where(x => x.IsDeleted == false && user.Id == x.UserId && x.IsConfirmed == true)
               .Include(x => x.Brand)
               .Include(x => x.Category)
               .Include(x => x.User)
               .OrderByDescending(x=>x.CreatedTime)
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
            {
                TempData["Error"] = "Miqdar düzgün daxil edilməyib.";
                return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                TempData["Error"] = "İstifadəçi tapılmadı.";
                return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });
            }

            // Bakiyeye ekleme işlemi
            user.Balance += amount.Value;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Məbləğ uğurla əlavə edildi!";
            return RedirectToAction("Index", "Profile", new { username = User.Identity!.Name });
        }


        [HttpPost]
        public async Task<IActionResult> Mexaric(string? username, int? amount)
        {
            if (!amount.HasValue || amount <= 0)
            {
                TempData["Error"] = "Miqdar düzgün daxil edilməyib.";
                return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                TempData["Error"] = "İstifadəçi tapılmadı.";
                return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });
            }

            if (user.Balance >= amount)
            {
                user.Balance -= amount.Value;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Məbləğ uğurla çıxarıldı!";
            }
            else
            {
                TempData["Error"] = "Yeterli balans yoxdur.";
            }

            return RedirectToAction("Index", "Profile", new { username = User.Identity.Name });
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
                return BadRequest();

            // Kullanıcıyı bul
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                return NotFound();


            // İlanı bul
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(ad => ad.Id == adId);
            if (advertisement == null)
            {
                return NotFound();
            }

            // İlanın durumunu VIP yap
            advertisement.Status = AdvertisementStatus.VIP;
            advertisement.VipStarted = DateTime.UtcNow;
            advertisement.VipEnded = advertisement.VipStarted.AddDays(7);



            // Kullanıcının bakiyesinden 5 birim düşür
            user.Balance -= 5;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();


            // Başarılı işlem sonrası Profile sayfasına yönlendir
            return Json(new { message = "Elaniniz Vip oldu!" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAdvertisement(string username, int adId)
        {
            // Kullanıcıyı bul
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return NotFound();
            }

            // İlanı bul
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(ad => ad.Id == adId);
            if (advertisement == null)
            {
                return NotFound();
            }

            // İlanı sil (isDeleted alanını true yapabilirsin)
            advertisement.IsDeleted = true;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarılı işlem mesajı
            return Json(new { message = "Elan ugurla silindi." });
        }

        

        public async Task<IActionResult> Messages(string? username)
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

            var question = await _context.Questions.Include(x => x.User).Where(x => x.UserId == user.Id).ToListAsync();


            ViewBag.Questions = question;

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

        public async Task<IActionResult> Tarixce(string? username)
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

            var medaxil = await _context.Odenis.Where(x => x.UserId == user.Id).ToListAsync();

            var mexaric = await _context.Cixaris.Where(x => x.UserId == user.Id).ToListAsync();

            ViewBag.Medaxil = medaxil;

            ViewBag.Mexaric = mexaric;

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









    }
}