using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.BL.DTOs.Question;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Controllers
{
    public class ContactController(AppDbContext _context,UserManager<User> _userManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(QuestionCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Zəhmət olmasa bütün sahələri düzgün doldurun!";
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            var question = new Question
            {
                UserId = userId,
                QuestionText = dto.Question,
                CreatedTime = DateTime.UtcNow
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sualınız uğurla göndərildi!";
            return RedirectToAction(nameof(Index));
        }



    }
}
