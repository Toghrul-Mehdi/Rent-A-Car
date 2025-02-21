using Rent_A_Car.BL.DTOs.Booking;
using Rent_A_Car.CORE.Entities;

namespace Rent_A_Car.MVC.VM
{
    public class AdvertisementDetailViewModel
    {
        public Advertisement Advertisement { get; set; }
        public BookingDTO BookingDTO { get; set; }
    }

}
