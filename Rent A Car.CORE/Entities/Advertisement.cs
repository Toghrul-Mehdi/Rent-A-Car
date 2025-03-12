using Rent_A_Car.CORE.Entities.Common;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Advertisement : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<CarImages> Images { get; set; }
        public int ViewCount { get; set; }
        public int Like {  get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public City City { get; set; }
        public string PhoneNumber { get; set; }   
        public int MinimalGunSayi { get; set; }
        public int MinimalSuruculukVesiqesi { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }        
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string Model { get; set; }
        public Fuel FuelType { get; set; }
        public Color Color { get; set; }
        public AdvertisementStatus Status { get; set; }
        public IEnumerable<WishList> WishList { get; set; }
        public DateTime VipStarted { get; set; }
        public DateTime VipEnded { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
