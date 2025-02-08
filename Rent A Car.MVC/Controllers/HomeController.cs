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
            return View(await _context.Categories.Where(x=>x.IsDeleted==false).ToListAsync());
        }        
    }
}
