using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class BookinGetAdminDTO
    {
       public string CounselorEmail {  get; set; }


        public string StudentEmail { get; set; } 
            
      
       
        public string CounselorName { get; set; }
       
       
    
        public Guid booking_id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CounselorUserId { get; set; }
        public string preferd_time { get; set; }
        public DateTime preferd_date { get; set; }
        public string status { get; set; }
     
        public decimal fee { get; set; }
    }
}
