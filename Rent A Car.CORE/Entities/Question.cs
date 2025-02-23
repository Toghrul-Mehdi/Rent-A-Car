using Rent_A_Car.CORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Entities
{
    public class Question : BaseEntity
    {        
        public string UserId { get; set; } 
        public User User { get; set; }
        public string QuestionText { get; set; }      

        public string? AdminResponse { get; set; } 
        public DateTime? ResponseAt { get; set; }
        public bool IsAnswered { get; set; } = false;        
    }

}
