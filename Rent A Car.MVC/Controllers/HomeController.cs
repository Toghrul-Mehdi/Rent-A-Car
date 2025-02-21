using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {    
        public async Task<IActionResult> Index()
        {
            ViewBag.AdvCount = await _context.Advertisements.CountAsync();
            ViewBag.UserCount = await _context.Users.CountAsync();
            return View(await _context.Advertisements
                .Where(x => !x.IsDeleted
                    && x.Status == AdvertisementStatus.VIP
                    && x.VipEnded > DateTime.UtcNow)
                .OrderByDescending(x => x.VipStarted)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .ToListAsync());
        }

        public async Task<IActionResult> AddToFavourite(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var data = await _context.Advertisements.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (data == null)
                return NotFound();

            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var existingWish = await _context.WishLists
                                              .FirstOrDefaultAsync(w => w.UserId == userId && w.AdvertisementId == data.Id);

            if (existingWish != null)
            {
                
                _context.WishLists.Remove(existingWish);
                await _context.SaveChangesAsync();
                return Json(new { success = false, message = "Removed from Favourites" });
            }
            else
            {
                
                WishList wishList = new WishList
                {
                    UserId = userId,
                    AdvertisementId = data.Id,
                };

                await _context.AddAsync(wishList);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFavouriteStatus()
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false });

            var favouriteAds =  _context.WishLists
                                    .Where(w => w.UserId == userId)
                                    .Select(w => w.AdvertisementId)
                                    .ToArray();

            return Json(new { success = true, favouriteAds });
        }



        public IActionResult NotFound()
        {
            return View();
        }


    }
}
