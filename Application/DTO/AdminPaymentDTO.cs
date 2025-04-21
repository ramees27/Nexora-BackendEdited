using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AdminPaymentDTO
    {
        public decimal total_amount {  get; set; }
        public decimal counselor_amount { get; set; }
        public decimal commission_amount { get; set; }

    }
}
