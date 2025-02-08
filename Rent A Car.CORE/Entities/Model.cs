using Rent_A_Car.CORE.Entities.Common;

namespace Rent_A_Car.CORE.Entities
{
    public class Model : BaseEntity
    {
        public string Name { get; set; }        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }        
    }
}
