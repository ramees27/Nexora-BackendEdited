using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class EducationDTO
    {
        
    public Guid education_id { get; set; }
        public Guid counselor_id { get; set; }
        public string qualification { get; set; }
        public string certificate_image_url { get; set; }
    }
}

