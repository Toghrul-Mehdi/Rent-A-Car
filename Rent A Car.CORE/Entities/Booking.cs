using Rent_A_Car.CORE.Entities.Common;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Booking : BaseEntity
    {
        public string UserID { get; set; }  // Burada UserID kullanıyoruz
        public User User { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public TimeSpan DropoffTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public PickupLocation PickupLocation { get; set; }
    }
}
