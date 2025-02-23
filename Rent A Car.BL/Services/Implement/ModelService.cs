using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.BL.DTOs.Vehicle;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.Services.Implement
{
    public class ModelService(AppDbContext _context) : IModelService
    {
        public async Task CreateAsync(ModelCreateDTO dto)
        {
            Model model = new Model
            {
                Name = dto.Name,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,               

            };
            await _context.Models.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var data = await _context.Models.FirstOrDefaultAsync(c => c.Id == id);
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Model>> GetAllAsync()
        {
            return await _context.Models.Where(x => x.IsDeleted == false).Include(x => x.Category).Include(x=>x.Brand).Where(x=>x.Brand.IsDeleted==false).Where(x => x.Category.IsDeleted == false).ToListAsync();
        }

        public async Task<ModelUpdateDTO> GetByDataAsync(int? id)
        {
            var data = await _context.Models
                .Where(x => x.Id == id)
                .Select(x => new ModelUpdateDTO
                {
                    Name = x.Name,
                    BrandId=x.BrandId,
                    CategoryId = x.CategoryId,                    
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> GetByIdAsync(int? id)
        {
            if (await _context.Models.AnyAsync(x => x.Id == id.Value))
            {
                return true;
            }
            return false;
        }

        public Task<bool> GetExistsAsync(ModelCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetExistsAsync(ModelUpdateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(ModelUpdateDTO dto, int? id)
        {
            var data = await _context.Models.Where(x => x.Id == id.Value).FirstOrDefaultAsync();

            if (data == null)
            {
                return false;
            }
            data.Name = dto.Name;
            data.BrandId = dto.BrandId;
            data.CategoryId = dto.CategoryId;            

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> ViewBagCategory()
        {
            var data = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            return data;
        }

        public async Task<List<Brand>> ViewBagBrand()
        {
            var data = await _context.Brands.Where(x => x.IsDeleted == false).ToListAsync();
            return data;
        }
    }
}
