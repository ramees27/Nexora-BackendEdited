using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CounselorStatusDTO
    {
        public bool Exists { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
    }


}
