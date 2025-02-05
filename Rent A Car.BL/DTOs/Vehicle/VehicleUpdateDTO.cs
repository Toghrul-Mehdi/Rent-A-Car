using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Vehicle
{
    public class VehicleUpdateDTO
    {
        [Required, MaxLength(32)]
        public string Marka { get; set; }
        [Required, MaxLength(32)]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? ColorId { get; set; }
        [Required]
        public Fuel FuelType { get; set; }
        public Color? Color => ColorId.HasValue ? (Color)ColorId.Value : (Color?)null;
    }
}
