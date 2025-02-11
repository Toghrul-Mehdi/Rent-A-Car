using Rent_A_Car.CORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public List<Model> Models { get; set; }
    }
}
