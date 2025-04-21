using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class BookingRescheduleDTO
    {
        public Guid BookingId { get; set; }
        public string PreferdTime { get; set; }
        public DateTime PreferdDate { get; set; }
    }
}
