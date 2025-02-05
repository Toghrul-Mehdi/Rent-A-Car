using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.BL.DTOs.Vehicle;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VehicleController(IVehicleService _service,AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }
        
        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await _context.Categories.Where(x=>x.IsDeleted==false).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            ViewBag.Category = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }            
            await _service.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (!await _service.DeleteAsync(id))
            {
                return NotFound();
            }
            else
            {
                await _service.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (!await _service.GetByIdAsync(id))
            {
                return NotFound();
            }

            return View(await _service.GetByDataAsync(id));
        }

        [HttpPost]

        public async Task<IActionResult> Update(VehicleUpdateDTO dto, int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(dto);
            }            
            await _service.UpdateAsync(dto, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
