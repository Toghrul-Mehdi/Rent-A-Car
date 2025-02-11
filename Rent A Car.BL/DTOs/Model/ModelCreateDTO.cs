using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.BL.DTOs.Vehicle
{
    public class ModelCreateDTO
    {
        [Required,MaxLength(32)]        
        public string Name { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
