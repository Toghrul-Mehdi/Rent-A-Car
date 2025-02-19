using Microsoft.AspNetCore.Http;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.BL.DTOs.Advertisement
{
    public class AdvertisementCreateDTO
    {
        [Required,MaxLength(64)]
        public string Title { get; set; }
        [Required, MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public IFormFile? CoverImage { get; set; }
        [Required]
        public ICollection<IFormFile>? OtherFiles { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Year { get; set; }
        public City CityName { get; set; } 
        public int PhoneNumber { get; set; }
        [Required]
        public int MinimalGunSayi { get; set; }
        [Required]
        public int MinimalSuruculukVesiqesi { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string Model { get; set; }
        public string UserId { get; set; } = null!;
        public User User { get; set; }
        public Fuel FuelType { get; set; } 
        public Color Color { get; set; } 
        public AdvertisementStatus Status { get; set; } 
    }
}
