using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AdminPaymentDetailsDTO
    {
        
            public Guid booking_id { get; set; }
            
            public string student_username { get; set; }
            public string counselor_name { get; set; }
            public string upi_id { get; set; }
            public DateTime preferd_date { get; set; }
            public string preferd_time { get; set; }
            public string booking_status { get; set; }
            public string admin_payout_status { get; set; }
            public decimal counselor_amount { get; set; } 
           public decimal total_amount {  get; set; }
            public decimal commission_amount {  get; set; }
            public DateTime paidByStudent {  get; set; }
            public DateTime paid_on {  get; set; }
            public bool is_active {  get; set; }
        public string status {  get; set; }
        public bool payment_status {  get; set; }

    }
}
