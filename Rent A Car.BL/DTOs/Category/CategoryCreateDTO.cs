using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required,MaxLength(16)]
        public string CategoryName { get; set; }
    }
}
