using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Controllers
{
    public class ContactController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
