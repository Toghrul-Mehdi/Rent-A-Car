using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Controllers
{
    public class CarController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.MaxPrice = await _context.Advertisements.MaxAsync(x=>x.Price);
            ViewBag.Category = await _context.Categories.Where(x=>x.IsDeleted==false).ToListAsync();
            return View(await _context.Advertisements
                .Where(x=>x.IsDeleted==false)
                .Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Include(x=>x.User)
                .ToListAsync());
        }
    }
}
