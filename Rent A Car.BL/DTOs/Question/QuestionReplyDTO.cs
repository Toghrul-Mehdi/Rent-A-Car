using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.BL.DTOs.Question
{
    public class QuestionReplyDTO
    {
        public int QuestionId { get; set; }
        [Required]
        public string AdminResponse { get; set; }
    }
}
