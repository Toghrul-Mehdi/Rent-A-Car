using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class CarController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.MaxPrice = _context.Advertisements
            .Where(x => x.Price != null && x.IsDeleted==false) 
            .AsEnumerable() 
            .Select(x => x.Price)
            .DefaultIfEmpty(0) 
            .Max();




            ViewBag.Category = await _context.Categories.Where(x=>x.IsDeleted==false).ToListAsync();
            ViewBag.Brand = await _context.Brands
                        .Where(x => x.IsDeleted == false)
                        .OrderBy(x => x.Name) 
                        .ToListAsync();

            return View(await _context.Advertisements
                .Where(x=>x.IsDeleted==false && x.IsConfirmed == true)
                .Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Include(x=>x.User)
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

            var favouriteAds = _context.WishLists
                                    .Where(w => w.UserId == userId)
                                    .Select(w => w.AdvertisementId)
                                    .ToArray();

            return Json(new { success = true, favouriteAds });
        }
    }
}
