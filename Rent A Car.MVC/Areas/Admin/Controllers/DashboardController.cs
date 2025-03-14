using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DashboardController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Questions = await _context.Questions.CountAsync();
            ViewBag.Profit = await _context.Bookings.SumAsync(x=>x.TotalPrice);
            ViewBag.Orders = await _context.Bookings
                .Where(x => x.CreatedTime.Date == DateTime.UtcNow.Date)
                .CountAsync();
            ViewBag.Users = await _context.Users               
                .CountAsync();
            return View();
        }
    }
}
