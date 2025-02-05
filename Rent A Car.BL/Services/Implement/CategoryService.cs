using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.BL.Services.Implement
{
    public class CategoryService(AppDbContext _context) : ICategoryService
    {        
        public async Task<List<Category>> GetAllAsync()
        {            
            return await _context.Categories.Where(c => c.IsDeleted==false).ToListAsync();
        }

        public async Task CreateAsync(CategoryCreateDTO dto)
        {             
            var category = new Category
            {
                CategoryName = dto.CategoryName,                
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();            
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var data = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryUpdateDTO?> GetByDataAsync(int? id)
        {         

            var data = await _context.Categories
                .Where(x => x.Id == id)
                .Select(x => new CategoryUpdateDTO
                {
                    CategoryName = x.CategoryName
                })
                .FirstOrDefaultAsync();

            return data; 
        }

        public async Task<bool> GetExistsAsync(CategoryCreateDTO dto)
        {
            if(await  _context.Categories.AnyAsync(x=>x.CategoryName == dto.CategoryName))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> GetExistsAsync(CategoryUpdateDTO dto)
        {
            if (await _context.Categories.AnyAsync(x => x.CategoryName == dto.CategoryName))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateAsync(CategoryUpdateDTO dto,int? id)
        {
            var data = await _context.Categories.Where(x => x.Id == id.Value).FirstOrDefaultAsync();

            if (data == null)
            {
                return false;
            }
            data.CategoryName = dto.CategoryName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GetByIdAsync(int? id)
        {
            if(await _context.Categories.AnyAsync(x=>x.Id==id.Value))
            {
                return true;
            }
            return false;
        }
    }

}
