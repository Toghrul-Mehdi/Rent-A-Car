using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Advertisement;
using Rent_A_Car.BL.DTOs.Booking;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Extension;
using Rent_A_Car.MVC.VM;
using System.Security.Claims;
using System.Text.Json;

namespace Rent_A_Car.MVC.Controllers
{
    public class BookingController(AppDbContext _context,IWebHostEnvironment _env) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int? advertisementId)
        {
            if (!advertisementId.HasValue)
            {
                return BadRequest();
            }

            // İlanı veritabanından çek
            var advertisement = await _context.Advertisements
                .Include(a => a.User)
                .SingleOrDefaultAsync(a => a.Id == advertisementId.Value);

            if (advertisement == null)
            {
                return NotFound();
            }

            // ViewModel oluştur ve ilgili alanları doldur
            var viewModel = new AdvertisementDetailViewModel
            {
                Advertisement = advertisement,
                BookingDTO = new BookingDTO
                {
                    AdvertisementId = advertisement.Id,
                    UserID = advertisement.User.Id // Kiralayan kişi
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookingDTO bookingDTO)
        {
            // 1. İlanın var olup olmadığını kontrol et
            var advertisement = await _context.Advertisements
                .Include(x => x.User)
                .FirstOrDefaultAsync(a => a.Id == bookingDTO.AdvertisementId);

            if (advertisement == null)
            {
                TempData["Error"] = "İcarəyə verilən maşın tapılmadı!";
                return RedirectToAction("Index", "Home");
            }

            // 2. Kullanıcı bilgilerini al
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            if (user == null)
            {
                TempData["Error"] = "İstifadəçi tapılmadı! Lütfən yenidən daxil olun.";
                return RedirectToAction("Login", "Account");
            }

            // 3. Kiralama süresini hesapla
            TimeSpan rentalDuration = bookingDTO.EndDate - bookingDTO.StartDate;
            int totalDays = (int)Math.Ceiling(rentalDuration.TotalDays);

            if (totalDays <= 0)
            {
                TempData["Error"] = "Vaxt araligini dogru daxil edin!";
                return RedirectToAction("Index", "Booking", new { advertisementId = bookingDTO.AdvertisementId });

            }

            // 4. Ödenecek ücreti hesapla
            decimal totalPrice = advertisement.Price * totalDays;

            // 5. Kullanıcının bakiyesini kontrol et
            if (user.Balance < totalPrice)
            {
                TempData["Error"] = "Balansınız kifayət qədər deyil! Zəhmət olmasa artırın.";
                return RedirectToAction("Index", "Booking", new { advertisementId = bookingDTO.AdvertisementId });

            }

            // 6. Kullanıcının bakiyesini düş
            user.Balance -= totalPrice;

            // 7. İlan sahibinin bakiyesini artır
            advertisement.User.Balance += totalPrice;

            // 8. Kullanıcı ve ilan sahibinin bakiyelerini güncelle
            _context.Users.Update(user);
            _context.Users.Update(advertisement.User);

            // 9. Kiralama kaydını oluştur
            var booking = new Booking
            {
                PickupLocation = bookingDTO.PickupLocation,
                StartDate = bookingDTO.StartDate,
                PickupTime = bookingDTO.PickupTime,
                DropoffTime = bookingDTO.DropoffTime,
                EndDate = bookingDTO.EndDate,
                AdvertisementId = bookingDTO.AdvertisementId,
                UserID = user.Id,
                TotalPrice = totalPrice,
                PhoneNumber = bookingDTO.PhoneNumber,
            };

            _context.Bookings.Add(booking);

            // 10. Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                TempData["Error"] = "Giriş zamanı problem yarandı. Yenidən daxil olun!";
                return RedirectToAction("Login", "Account");
            }

            TempData["Success"] = "İcarə prosesi uğurla tamamlandı!";
            return RedirectToAction("Index", "Profile", new { username });
        }



        [HttpGet]
        public async Task<IActionResult> GetUnavailableDates(int advertisementId)
        {
            var bookedDates = await _context.Bookings
                .Where(b => b.AdvertisementId == advertisementId)
                .Select(b => new
                {
                    StartDate = b.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = b.EndDate.ToString("yyyy-MM-dd")
                })
                .ToArrayAsync(); // JSON ARRAY DÖNDÜRMESİ İÇİN EKLENDİ!

            return new JsonResult(bookedDates, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }); // System.Text.Json KULLANIYORUZ
        }






    }
}
