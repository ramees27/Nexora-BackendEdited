using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ReviewAddDTO
    {
        public Guid counselor_id {  get; set; }
        public Guid booking_id {  get; set; }
        public int rating { get; set; } 
        public string review { get; set; }
    }
}
