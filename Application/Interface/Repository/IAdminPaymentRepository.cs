using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interface.Repository
{
    public interface IAdminPaymentRepository
    {
        Task<PaymentSummaryDTO> GetPaymentSummaryAsync();
        Task<List<AdminPaymentDetailsDTO>> GetPaymentDetailsAsync();
        Task<ReviewGetDTOStudent> GetReviewByBookingIdAsync(Guid bookingId);
        Task<bool> GetUpdatePaymentIntoCouncelor(Guid bookingId);
       
    }
}
