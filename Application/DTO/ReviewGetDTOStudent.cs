using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ReviewGetDTOStudent
    {
        
       
        
       
        public int rating { get; set; } // 1 to 5
        public string review { get; set; }
        public DateTime created_at {  get; set; }
       
        public Guid student_id { get; set; }
        public string username { get; set; }


    }
}
