using Microsoft.AspNetCore.Http;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Advertisement
{
    public class AdvertisementUpdateDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Title { get; set; }
        [Required, MaxLength(400)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile? CoverImage { get; set; }

        public ICollection<CarImages> ImagesUrl { get; set; }  

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
