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

            var orders = await _context.Bookings.Include(x=>x.Advertisement)
                .ThenInclude(x=>x.Brand)
                .Include(x=>x.Advertisement)
                .ThenInclude(x=>x.Category)
                .Include(x=>x.User)
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                ViewBag.Orders = new List<Booking>();
            }
            else
            {
                ViewBag.Orders = orders;
            }

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
            ViewBag.AdvCount = await _context.Advertisements.Include(x=>x.User).Where(x=>x.UserId == userId).CountAsync();
            ViewBag.OrderCount = await _context.Bookings.Include(x => x.User).Where(x=>x.UserID == userId).CountAsync(); 
            ViewBag.FavCount = await _context.WishLists
                .Where(x => x.UserId == userId)
                .Include(x => x.Advertisement)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Advertisement)                
                .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.Advertisement.IsDeleted == false)
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
   .Where(x => x.Advertisement.IsDeleted == false) 
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
            advertisement.VipStarted = DateTime.UtcNow;
            advertisement.VipEnded = advertisement.VipStarted.AddDays(7);



            // Kullanıcının bakiyesinden 5 birim düşür
            user.Balance -= 5;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
            

            // Başarılı işlem sonrası Profile sayfasına yönlendir
            return Json(new { message = "VIP işlemi başarılı!" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAdvertisement(string username, int adId)
        {
            // Kullanıcıyı bul
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // İlanı bul
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(ad => ad.Id == adId);
            if (advertisement == null)
            {
                return NotFound("İlan bulunamadı.");
            }

            // İlanı sil (isDeleted alanını true yapabilirsin)
            advertisement.IsDeleted = true;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarılı işlem mesajı
            return Json(new { message = "İlan başarıyla silindi." });
        }

        /*[HttpGet]
        public async Task<IActionResult> Update(string username, int adId)
        {
            var brands = await _context.Brands
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            ViewBag.Brand = brands;
            // Kullanıcıyı doğrula
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return Unauthorized("Geçersiz kullanıcı!");
            }

            // İlanı kontrol et
            var advertisement = await _context.Advertisements.Include(x=>x.Brand).Include(x=>x.Images)
                .FirstOrDefaultAsync(a => a.Id == adId && a.UserId == user.Id);

            if (advertisement == null)
            {
                return NotFound("İlan bulunamadı veya bu ilana erişiminiz yok!");
            }

            var updateDTO = new AdvertisementUpdateDTO
            {
                Id = adId,
                Title = advertisement.Title,
                Description = advertisement.Description,
                ImageUrl = advertisement.ImageUrl,
                ImagesUrl = advertisement.Images,
                Price = advertisement.Price,
                Year = advertisement.Year,
                CityName = advertisement.City,
                PhoneNumber = advertisement.PhoneNumber,
                MinimalGunSayi = advertisement.MinimalGunSayi,
                MinimalSuruculukVesiqesi = advertisement.MinimalSuruculukVesiqesi,
                BrandId = advertisement.BrandId,
                Model = advertisement.Model,
                FuelType = advertisement.FuelType,
                Color = advertisement.Color                
            };

            return View(updateDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdvertisementUpdateDTO model, int adId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Güncellenen ilanı veritabanından al
            var advertisement = await _context.Advertisements
                .Include(a => a.Images) // Diğer resimleri de getir
                .FirstOrDefaultAsync(a => a.Id == adId);

            if (advertisement == null)
            {
                return NotFound("İlan bulunamadı.");
            }

            // 2. Ana Kapak Fotoğrafı Güncelleme
            if (model.CoverImage != null)
            {
                // Yeni resmi kaydet
                var uploadsFolder = Path.Combine(_env.WebRootPath, "img/cars");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CoverImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(fileStream);
                }

                // Eski resmi sil (eğer yeni yükleme varsa)
                if (!string.IsNullOrEmpty(advertisement.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, advertisement.ImageUrl.TrimStart('~', '/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Yeni resmi kaydet
                advertisement.ImageUrl = "/img/cars/" + uniqueFileName;
            }
            else
            {
                // Kullanıcı yeni bir fotoğraf yüklemediyse eski fotoğrafı koru
                advertisement.ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? advertisement.ImageUrl : model.ImageUrl;
            }

            // 3. Diğer Resimler (Galeri) Güncelleme
            if (model.OtherFiles != null && model.OtherFiles.Count > 0)
            {
                // Yeni resimler ekleniyor
                foreach (var file in model.OtherFiles)
                {
                    if (file != null)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(_env.WebRootPath, "img/cars", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Yeni resimleri veritabanına ekle
                        var newOtherImage = new CarImages
                        {
                            AdvertisementId = advertisement.Id,
                            OtherImageUrl = "/img/cars/" + uniqueFileName
                        };
                        _context.CarImages.Add(newOtherImage);
                    }
                }
            }

            // 4. Diğer Alanları Güncelle
            advertisement.Title = model.Title;
            advertisement.Description = model.Description;
            advertisement.Price = model.Price;
            advertisement.Year = model.Year;
            advertisement.City = model.CityName;
            advertisement.PhoneNumber = model.PhoneNumber;
            advertisement.MinimalGunSayi = model.MinimalGunSayi;
            advertisement.MinimalSuruculukVesiqesi = model.MinimalSuruculukVesiqesi;
            advertisement.BrandId = model.BrandId;
            advertisement.Model = model.Model;
            advertisement.UserId = model.UserId;
            advertisement.FuelType = model.FuelType;
            advertisement.Color = model.Color;
            advertisement.Status = model.Status;

            // 5. Değişiklikleri Kaydet
            _context.Advertisements.Update(advertisement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }*/

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

            var question = await _context.Questions.Include(x=>x.User).Where(x=>x.UserId==user.Id).ToListAsync();
            

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










    }
}
