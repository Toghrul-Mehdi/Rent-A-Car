using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
    }
}
