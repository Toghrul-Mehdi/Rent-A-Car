using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class MexaricController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cixaris
    .Include(x => x.User)
    .OrderBy(x => x.IsConfirmed) // Öncelikle IsConfirmed == false olanları getir
    .ToListAsync());
        }
        public async Task<IActionResult> Confirm(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            // İlanı bul
            var cixaris = await _context.Cixaris.FirstOrDefaultAsync(x => x.Id == id);
            if (cixaris == null)
            {
                return NotFound();
            }


            cixaris.IsConfirmed = true;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
