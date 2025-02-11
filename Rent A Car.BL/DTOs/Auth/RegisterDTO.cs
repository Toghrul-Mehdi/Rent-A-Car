using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required, MaxLength(32)]
        public string Fullname { get; set; }
        [Required, MaxLength(32)]
        public string Username { get; set; }
        [Required, MaxLength(40), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(32), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(32), DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
