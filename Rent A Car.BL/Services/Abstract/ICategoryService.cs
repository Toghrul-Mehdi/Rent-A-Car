using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.Services.Abstract
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateDTO dto);
        Task<List<Category>> GetAllAsync();
        Task<CategoryUpdateDTO> GetByDataAsync(int? id);        
        Task<bool> DeleteAsync(int? id);
        Task<bool> GetExistsAsync(CategoryCreateDTO dto);
        Task<bool> GetExistsAsync(CategoryUpdateDTO dto);
        Task<bool> UpdateAsync(CategoryUpdateDTO dto,int? id);
        Task<bool> GetByIdAsync(int? id);
    }
}
