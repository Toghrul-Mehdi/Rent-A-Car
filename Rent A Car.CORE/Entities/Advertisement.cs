using Rent_A_Car.CORE.Entities.Common;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Advertisement : BaseEntity
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int Id { get; set; } 
        public User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }         
        public AdvertisementStatus Status { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public DateTime? ExpiryDate { get; set; } 
    }
}
