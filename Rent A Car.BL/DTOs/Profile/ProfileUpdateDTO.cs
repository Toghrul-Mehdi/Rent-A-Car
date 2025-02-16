using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Profile
{
    public class ProfileUpdateDTO
    {
        [MaxLength(32)]
        public string Username { get; set; }
        [MaxLength(32),EmailAddress]
        public string Email { get; set; }
        [MaxLength(32)]
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
