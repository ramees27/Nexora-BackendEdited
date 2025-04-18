using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }  // ✅ Updated case
        public string Role { get; set; }       // ✅ Updated case
        public string PasswordHash { get; set; }
        public string Refresh_Token { get; set; }  // ✅ Match DB column
        public DateTime Token_Expiry { get; set; } 
        public bool Is_Deleted { get; set; }       
    }

}
