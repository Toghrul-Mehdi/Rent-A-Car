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
        public decimal Balance { get; set; }
        public List<Advertisement> Advertisements { get; set; }
        public IEnumerable<WishList> WishList { get; set; }  
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Odenis> Odenisler { get; set; }
        public IEnumerable<Cixaris> Cixarislar { get; set; }
    }
}
