using Rent_A_Car.CORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Odenis : BaseEntity
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int Mebleg {  get; set; }
    }
}
