using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
      
        
            public Guid rating_id { get; set; }
            public Guid student_id { get; set; }
            public Guid counselor_id { get; set; }
            public Guid booking_id { get; set; }
            public int rating { get; set; } // 1 to 5
            public string review { get; set; }
         
        

    }
}
