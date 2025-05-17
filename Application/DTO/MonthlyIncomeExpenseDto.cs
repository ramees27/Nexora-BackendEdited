using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class MonthlyIncomeExpenseDto
    {
        public string Month { get; set; } // "2025-05"
        public decimal Expense { get; set; }
        public decimal Revenue { get; set; }
    }
}
