using Microsoft.AspNetCore.Http;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Booking
{
    public class BookingDTO
    {
        public string UserID { get; set; }  // User ID
        public int AdvertisementId { get; set; }  // İlan ID
        public DateTime StartDate { get; set; }  // Kiralama başlangıç tarihi
        public DateTime EndDate { get; set; }  // Kiralama bitiş tarihi
        public TimeSpan PickupTime { get; set; }  // Pickup zamanı
        public TimeSpan DropoffTime { get; set; }  // Dropoff zamanı
        public PickupLocation PickupLocation { get; set; }  // Pickup Lokasyonu
        public BookingStatus Status { get; set; }  // Kiralama durumu (örneğin: "Pending", "Confirmed", "Cancelled")
        public string PhoneNumber { get; set; }
    }

}
