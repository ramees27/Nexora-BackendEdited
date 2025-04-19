using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Booking
    {
        public Guid booking_id { get; set; }
        public Guid student_id { get; set; }
        public Guid counselor_id { get; set; }
        public string preferd_time { get; set; }
        public DateTime preferd_date { get; set; }
        public string status { get; set; } 
        public DateTime created_at { get; set; }
    }

}
