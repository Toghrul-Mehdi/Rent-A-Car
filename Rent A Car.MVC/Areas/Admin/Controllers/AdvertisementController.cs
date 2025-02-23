using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdvertisementController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Advertisements.Include(x => x.Category).Include(x => x.Brand).Include(x => x.User).Where(x => x.IsDeleted == false).ToListAsync());
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if(!id.HasValue)
            {
                return BadRequest();
            }
            // İlanı bul
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(ad => ad.Id == id);
            if (advertisement == null)
            {
                return NotFound("İlan bulunamadı.");
            }

            // İlanı sil (isDeleted alanını true yapabilirsin)
            advertisement.IsDeleted = true;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
