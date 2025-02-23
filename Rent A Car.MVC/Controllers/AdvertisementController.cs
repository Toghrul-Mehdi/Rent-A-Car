using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Advertisement;
using Rent_A_Car.BL.DTOs.Booking;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Extension;
using Rent_A_Car.MVC.VM;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class AdvertisementController(AppDbContext _context,IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var brands = await _context.Brands
                .Where(x => x.IsDeleted == false)
                .ToListAsync();                

                ViewBag.Brand = brands;

                

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdvertisementCreateDTO dto)
        {
            var brands = await _context.Brands
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            
            ViewBag.Brand = brands;




            if (dto.CoverImage != null && dto.CoverImage.Length > 0) 
            {
                if (string.IsNullOrEmpty(dto.CoverImage.ContentType) || !dto.CoverImage.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("CoverImage", "File must be an image!");
                }
                if (dto.CoverImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverImage", "File must be less than 5MB!");
                }
            }
            else
            {
                ModelState.AddModelError("CoverImage", "Cover image is required.");
            }

            if (dto.OtherFiles != null && dto.OtherFiles.Any())
            {
                if (dto.OtherFiles.Count > 3)
                {
                    ModelState.AddModelError("OtherFiles", "You can upload a maximum of 3 images.");
                }

                if (dto.OtherFiles.Any(x => string.IsNullOrEmpty(x.ContentType) || !x.ContentType.StartsWith("image")))
                {
                    string fileNames = string.Join(',', dto.OtherFiles
                        .Where(x => string.IsNullOrEmpty(x.ContentType) || !x.ContentType.StartsWith("image"))
                        .Select(x => x.FileName));
                    ModelState.AddModelError("OtherFiles", fileNames + " is (are) not an image.");
                }

                if (dto.OtherFiles.Any(x => x.Length > 5 * 1024 * 1024))
                {
                    string fileNames = string.Join(',', dto.OtherFiles
                        .Where(x => x.Length > 5 * 1024 * 1024)
                        .Select(x => x.FileName));
                    ModelState.AddModelError("OtherFiles", fileNames + " is (are) bigger than 5MB.");
                }
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Brand = await _context.Brands
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();
                return View(dto);
            }


            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            int categoryId = await _context.Models.Where(x=>x.Name==dto.Model).Select(dto=>dto.CategoryId).FirstOrDefaultAsync();
            Advertisement advertisement = new Advertisement
            {
                Title = dto.Title,
                Description = dto.Description,
                BrandId = dto.BrandId,
                Model = dto.Model,
                Color = dto.Color,
                FuelType = dto.FuelType,
                City = dto.CityName,
                Price = dto.Price,
                CategoryId = categoryId,
                ViewCount = 0,
                Like = 0,
                PhoneNumber = dto.PhoneNumber,
                UserId = userId,
                Year = dto.Year,
                MinimalGunSayi = dto.MinimalGunSayi,
                MinimalSuruculukVesiqesi = dto.MinimalSuruculukVesiqesi,
                ImageUrl = await dto.CoverImage!.UploadAsync(_env.WebRootPath, "img", "cars"),
                Images = dto.OtherFiles!.Select(x => new CarImages
                {
                    OtherImageUrl = x.UploadAsync(_env.WebRootPath, "img", "cars").Result
                }).ToList()
            };
            await _context.Advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
            
        }

        [HttpGet]
        public async Task<JsonResult> GetModelsByBrandId(int brandId)
        {
            var models =  _context.Models
                .Where(x => x.BrandId == brandId && x.IsDeleted == false)
                .Select(x => new { x.Id, x.Name })
                .ToArray();

            return Json(models);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (!id.HasValue) return BadRequest("Geçersiz ilan ID'si!");

            var advertisement = await _context.Advertisements
                .Where(x => x.Id == id.Value)
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.User)
                .SingleOrDefaultAsync();

            if (advertisement == null) return NotFound("İlan bulunamadı!");

            advertisement.ViewCount++;
            await _context.SaveChangesAsync();

            // BookingDTO oluşturuluyor
            var bookingDTO = new BookingDTO
            {
                AdvertisementId = advertisement.Id
            };

            // ViewModel oluşturuluyor
            var viewModel = new AdvertisementDetailViewModel
            {
                Advertisement = advertisement,
                BookingDTO = bookingDTO
            };

            return View(viewModel);
        }


        
    }
}
