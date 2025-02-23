using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Question
{
    public class QuestionCreateDTO
    {
        [Required]
        public string Question {  get; set; }
    }
}
