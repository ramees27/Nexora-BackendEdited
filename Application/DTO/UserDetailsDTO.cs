using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserDetailsDTO
    {
       
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }  // ✅ Updated case
            public string Role { get; set; } 
          public DateTime created_at { get; set; }




            public bool is_deleted { get; set; }
        
    }
}
