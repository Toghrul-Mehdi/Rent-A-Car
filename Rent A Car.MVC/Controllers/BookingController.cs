using Microsoft.AspNetCore.Mvc;

namespace Rent_A_Car.MVC.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
