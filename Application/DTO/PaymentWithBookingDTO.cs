using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PaymentWithBookingDTO
    {
      
            public Guid payment_id { get; set; }
            public Guid booking_id { get; set; }
            public decimal total_amount { get; set; }
            public decimal commission_amount { get; set; }
            public decimal counselor_amount { get; set; }
            public string admin_payout_status { get; set; }
            public DateTime? paid_on { get; set; }
            public DateTime? paidByStudent { get; set; }

            public DateTime preferd_date { get; set; }
            public string preferd_time { get; set; }
            public string UserEmail { get; set; }
        public string status { get; set; }
        public bool payment_status {  get; set; }


    }
}
