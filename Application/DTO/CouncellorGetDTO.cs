using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CouncellorGetDTO
    {
        public Guid counselors_id { get; set; }
        public Guid user_id { get; set; }
        public string full_name { get; set; }
        public string specialization { get; set; }
        public string short_bio { get; set; }
        
        
        public int experience { get; set; }
        
       
        public int avg_rating { get; set; }
        public string image_url { get; set; }
       

        // From Users table
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
