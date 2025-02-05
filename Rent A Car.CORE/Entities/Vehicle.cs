using Rent_A_Car.CORE.Entities.Common;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Vehicle : BaseEntity
    {
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Color? Color { get; set; }    
        public Fuel FuelType { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
