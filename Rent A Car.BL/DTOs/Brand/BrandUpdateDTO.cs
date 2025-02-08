using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Brand
{
    public class BrandUpdateDTO
    {
        [Required, MaxLength(32)]
        public string Name { get; set; }
    }
}
