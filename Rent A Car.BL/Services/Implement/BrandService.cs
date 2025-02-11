using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Brand;
using Rent_A_Car.BL.DTOs.Category;
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
    public class BrandService(AppDbContext _context) : IBrandService
    {
        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.Where(c => c.IsDeleted == false).ToListAsync();
        }

        public async Task CreateAsync(BrandCreateDTO dto)
        {
            var brand = new Brand
            {
                Name = dto.Name,
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var data = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BrandUpdateDTO?> GetByDataAsync(int? id)
        {

            var data = await _context.Brands
                .Where(x => x.Id == id)
                .Select(x => new BrandUpdateDTO
                {
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> GetExistsAsync(BrandCreateDTO dto)
        {
            if (await _context.Brands.AnyAsync(x => x.Name == dto.Name))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> GetExistsAsync(BrandUpdateDTO dto)
        {
            if (await _context.Brands.AnyAsync(x => x.Name == dto.Name))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateAsync(BrandUpdateDTO dto, int? id)
        {
            var data = await _context.Brands.Where(x => x.Id == id.Value).FirstOrDefaultAsync();

            if (data == null)
            {
                return false;
            }
            data.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GetByIdAsync(int? id)
        {
            if (await _context.Brands.AnyAsync(x => x.Id == id.Value))
            {
                return true;
            }
            return false;
        }
    }
}
