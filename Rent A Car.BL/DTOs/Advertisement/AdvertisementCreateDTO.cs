using Microsoft.AspNetCore.Http;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;

namespace Rent_A_Car.BL.DTOs.Advertisement
{
    public class AdvertisementCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? CoverImage { get; set; }
        public ICollection<IFormFile>? OtherFiles { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public City CityName { get; set; } 
        public int PhoneNumber { get; set; }
        public int MinimalGunSayi { get; set; }
        public int MinimalSuruculukVesiqesi { get; set; }
        public int CategoryId { get; set; }               
        public int BrandId { get; set; }
        public string Model { get; set; }
        public string UserId { get; set; } = null!;
        public User User { get; set; }
        public Fuel FuelType { get; set; } 
        public Color Color { get; set; } 
        public AdvertisementStatus Status { get; set; } 
    }
}
