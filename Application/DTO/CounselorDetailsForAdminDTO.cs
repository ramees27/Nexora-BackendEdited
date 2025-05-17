using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CounselorDetailsDTO
    {
        public Guid counselors_id { get; set; }
        public Guid user_id { get; set; }
        public string full_name { get; set; }
        public string specialization { get; set; }
        public string short_bio { get; set; }
        public string mobile_number { get; set; }
        public string email {  get; set; }
        public int experience { get; set; }
        public DateTime created_at { get; set; }
        public int hourly_rate { get; set; }
        public string upi_id { get; set; }
        public int avg_rating { get; set; }
        public string image_url { get; set; }
        
        public string qualification { get; set; }
        public string certificate_image_url { get; set; }
        public bool is_verified {  get; set; }
       public bool is_deleted {  get; set; }

    }

}
