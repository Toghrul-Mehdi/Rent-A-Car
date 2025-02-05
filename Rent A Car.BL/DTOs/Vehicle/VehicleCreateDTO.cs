using Microsoft.AspNetCore.Http;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Vehicle
{
    public class VehicleCreateDto
    {
        public string Marka { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }        
        public bool IsAvailable { get; set; }
        public int Year { get; set; }
        public Color Color { get; set; }
        public Fuel FuelType { get; set; }
    }
}
