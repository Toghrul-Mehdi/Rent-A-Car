using Rent_A_Car.BL.DTOs.Brand;
using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.Services.Abstract
{
    public interface IBrandService
    {
        Task CreateAsync(BrandCreateDTO dto);
        Task<List<Brand>> GetAllAsync();
        Task<BrandUpdateDTO> GetByDataAsync(int? id);
        Task<bool> DeleteAsync(int? id);
        Task<bool> GetExistsAsync(BrandCreateDTO dto);
        Task<bool> GetExistsAsync(BrandUpdateDTO dto);
        Task<bool> UpdateAsync(BrandUpdateDTO dto, int? id);
        Task<bool> GetByIdAsync(int? id);
    }
}
