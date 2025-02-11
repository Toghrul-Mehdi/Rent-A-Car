using Rent_A_Car.BL.DTOs.Category;
using Rent_A_Car.BL.DTOs.Vehicle;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.Services.Abstract
{
    public interface IModelService
    {
        Task CreateAsync(ModelCreateDTO dto);
        Task<List<Model>> GetAllAsync();
        Task<ModelUpdateDTO> GetByDataAsync(int? id);
        Task<bool> DeleteAsync(int? id);
        Task<bool> GetExistsAsync(ModelCreateDTO dto);
        Task<bool> GetExistsAsync(ModelUpdateDTO dto);
        Task<bool> UpdateAsync(ModelUpdateDTO dto, int? id);
        Task<bool> GetByIdAsync(int? id);
        Task<List<Category>> ViewBagCategory();
        Task<List<Brand>> ViewBagBrand();
    }
}
