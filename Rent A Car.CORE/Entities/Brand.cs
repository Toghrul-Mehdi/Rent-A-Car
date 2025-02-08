using Rent_A_Car.CORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Brand : BaseEntity
    {
        public string Name {  get; set; }
        public IEnumerable<Model> Vehicles { get; set; }
    }
}
