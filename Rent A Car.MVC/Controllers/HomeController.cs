using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.MVC.Models;
using System.Diagnostics;

namespace Rent_A_Car.MVC.Controllers
{
    public class HomeController : Controller
    {       
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
