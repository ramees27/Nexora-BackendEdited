using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interface.Repository
{
    public interface IPaymentRepository
    {
        Task<List<PaymentWithBookingDTO>> GetAllPaymentsWithBookingAsync(Guid councelorId);
        Task<PaymentSummaryDTO> GetEarningsBreakdownAsync(Guid counselorId);
     

    }
}
