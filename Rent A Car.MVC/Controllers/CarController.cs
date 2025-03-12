using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DAL.Context;

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
    }
}
