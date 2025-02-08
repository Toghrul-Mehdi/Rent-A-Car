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
        /*public async Task CreateAsync(VehicleCreateDto dto)
        {
            Vehicle vehicle = new Vehicle
            {
                Marka = dto.Marka,
                Model = dto.Model,
                Year = dto.Year,
                CategoryId = dto.CategoryId,
                Color = dto.Color,
                FuelType = dto.FuelType,
            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var data = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == id);
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.Where(x=>x.IsDeleted==false).Include(x=>x.Category).ToListAsync();
        }

        public async Task<VehicleUpdateDTO> GetByDataAsync(int? id)
        {
            var data = await _context.Vehicles
                .Where(x => x.Id == id)
                .Select(x => new VehicleUpdateDTO
                {
                    Marka = x.Marka,
                    Model = x.Model,
                    Year = x.Year,
                    CategoryId = x.CategoryId,
                    ColorId = (int)x.Color,
                    FuelType =x.FuelType
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> GetByIdAsync(int? id)
        {
            if (await _context.Vehicles.AnyAsync(x => x.Id == id.Value))
            {
                return true;
            }
            return false;
        }

        public Task<bool> GetExistsAsync(VehicleCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetExistsAsync(VehicleUpdateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(VehicleUpdateDTO dto, int? id)
        {
            var data = await _context.Vehicles.Where(x => x.Id == id.Value).FirstOrDefaultAsync();

            if (data == null)
            {
                return false;
            }
            data.Marka = dto.Marka;
            data.Model = dto.Model;
            data.Year = dto.Year;
            data.CategoryId = dto.CategoryId;
            data.Color=dto.Color;
            data.FuelType = dto.FuelType;           

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> ViewBagAsync()
        {
            var data = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            return data;
        }*/
    }
}
