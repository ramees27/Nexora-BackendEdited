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
            public string Role { get; set; }       // ✅ Updated case
           
        
           
            public bool Is_Deleted { get; set; }
        
    }
}
