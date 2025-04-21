using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public  class BookingSectionDetailsDTO
    {
      
        
            public Guid booking_id { get; set; }
            public Guid student_id { get; set; }
            public Guid counselor_id { get; set; }
            public string preferd_time { get; set; }
            public DateTime preferd_date { get; set; }
            public string status { get; set; }
            public DateTime created_at { get; set; }
            public decimal fee { get; set; }
     
        public int rating { get; set; } // 1 to 5
        public string review { get; set; }
     
        public string username { get; set; }


    }
}
