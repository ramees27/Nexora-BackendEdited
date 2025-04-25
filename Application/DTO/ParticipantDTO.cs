using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class BookingParticipantsDto
    {
        public Guid student_id { get; set; }
        public Guid counselor_id { get; set; }
    }

}
