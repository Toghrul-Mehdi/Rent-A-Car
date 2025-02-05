using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.BL.Services.Abstract;

namespace Rent_A_Car.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(ICategoryService _service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create(CategoryCreateDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(!await _service.GetExistsAsync(dto))
            {
                ModelState.AddModelError("","Category has been exists.");
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
            if(!await _service.DeleteAsync(id))
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
            if(!await _service.GetByIdAsync(id))
            {
                return NotFound();
            }
            
            return View(await _service.GetByDataAsync(id));
        }

        [HttpPost]

        public async Task<IActionResult> Update(CategoryUpdateDTO dto, int? id)
        {
            if(!id.HasValue)
                return BadRequest();
            if(!ModelState.IsValid)
            {
                return View(dto);   
            }

            if (!await _service.GetExistsAsync(dto))
            {
                ModelState.AddModelError("", "Category has been exists or not changed(If you don't wanna change, go back).");
                return View();
            }
            await _service.UpdateAsync(dto,id);
            return RedirectToAction(nameof(Index));
        }
    }
}
