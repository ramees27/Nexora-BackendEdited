using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ActivityGetDTOForUser
    {
        public Guid booking_id { get; set; }
       
        public string preferd_time { get; set; }
        public DateTime preferd_date { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid counselor_id { get; set; }
        public Guid student_id { get; set; }
        public string full_name { get; set; }
        public string specialization { get; set; }
        public bool payment_status {  get; set; }
      
      
        public decimal fee { get; set; }
       
       
        public string image_url { get; set; }
       
    }
}

