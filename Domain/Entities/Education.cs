using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Education
    {
        public Guid education_id {  get; set; }
        public Guid counselor_id {  get; set; }
        public string qualification {  get; set; }
        public string certificate_image_url {  get; set; }
    }
}
