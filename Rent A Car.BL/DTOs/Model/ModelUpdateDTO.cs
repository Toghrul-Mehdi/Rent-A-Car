using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.BL.DTOs.Vehicle
{
    public class ModelUpdateDTO
    {
        [Required, MaxLength(32)]
        public string Marka { get; set; }
        [Required, MaxLength(32)]
        public string Model { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}