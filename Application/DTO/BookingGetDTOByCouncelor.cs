using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class BookingGetDTOByCouncelor
    {
        public Guid booking_id { get; set; }
        public string status {  get; set; }

        public string preferd_time { get; set; }
        public DateTime Preferd_Date { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public Guid counselor_id { get; set; }
        public Guid student_id { get; set; }
        
        
        public bool payment_status { get; set; }


        public decimal fee { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public bool is_active {  get; set; }



    }
}
