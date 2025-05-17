using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PaymentSummaryDTO
    {
        
        
            public decimal TotalAmount { get; set; }
            public decimal PendingAmount { get; set; }
            public decimal ReceivedAmount { get; set; }
        public decimal CounselorAmount {  get; set; }
        public decimal PaidAmount {  get; set; }
        public decimal CommisionAmout { get; set; }




    }
}
