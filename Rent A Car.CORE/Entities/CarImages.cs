using Rent_A_Car.CORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class CarImages : BaseEntity
    {
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public string OtherImageUrl { get; set; }
    }
}
