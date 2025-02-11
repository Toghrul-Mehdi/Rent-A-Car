using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.BL.DTOs.Vehicle;
using Rent_A_Car.BL.Services.Abstract;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModelController(IModelService _service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await _service.ViewBagCategory();
            ViewBag.Brand = await _service.ViewBagBrand();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ModelCreateDTO dto)
        {
            ViewBag.Category = await _service.ViewBagCategory();
            ViewBag.Brand = await _service.ViewBagBrand();
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
            ViewBag.Category = await _service.ViewBagCategory();
            ViewBag.Brand = await _service.ViewBagBrand();
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

        public async Task<IActionResult> Update(ModelUpdateDTO dto, int? id)
        {
            ViewBag.Category = await _service.ViewBagCategory();
            ViewBag.Brand = await _service.ViewBagBrand();
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
