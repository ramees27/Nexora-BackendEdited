using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTO
{
    public class EducationCreateDTO
    {
        public Guid CounselorId { get; set; }
        public string Qualification { get; set; }
        public IFormFile CertificateImage { get; set; }
    }

}
