using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Models;
using System.Diagnostics;

namespace Rent_A_Car.MVC.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {     
        public async Task<IActionResult> Index()
        {
            ViewBag.UserCount = await _context.Users.CountAsync();
            return View(await _context.Advertisements.Where(x=>x.IsDeleted==false).Include(x=>x.Category).Include(x=>x.Brand).ToListAsync());
        }        
    }
}
