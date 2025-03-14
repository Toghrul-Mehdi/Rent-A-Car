using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Question;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class MailController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.UnReadQuestion = await _context.Questions.Include(x => x.User).Where(x => x.IsAnswered == false).CountAsync();
            ViewBag.Question = await _context.Questions.Include(x=>x.User).Where(x=>x.IsAnswered==false).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReplyQuestion(QuestionReplyDTO dto)
        {
            var question = await _context.Questions.FindAsync(dto.QuestionId);

            if (question == null)
            {
                TempData["ErrorMessage"] = "Sual tapılmadı!";
                return RedirectToAction(nameof(Index));
            }

            question.AdminResponse = dto.AdminResponse;
            question.ResponseAt = DateTime.UtcNow;
            question.IsAnswered = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cavabınız uğurla göndərildi!";
            return RedirectToAction(nameof(Index));
        }


    }
}
