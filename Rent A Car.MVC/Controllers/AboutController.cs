using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Controllers
{
    public class AboutController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.AdvCount = await _context.Advertisements.CountAsync();
            ViewBag.OrderCount = await _context.Bookings.CountAsync();
            ViewBag.UserCount = await _context.Users.CountAsync();
            return View();
        }
    }
}
