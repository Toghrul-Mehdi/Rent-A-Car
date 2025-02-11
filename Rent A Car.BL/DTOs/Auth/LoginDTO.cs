using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Auth
{
    public class LoginDTO
    {
        [Required, MaxLength(32)]
        public string UsernameOrEmail { get; set; }
        [Required, MaxLength(32), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
