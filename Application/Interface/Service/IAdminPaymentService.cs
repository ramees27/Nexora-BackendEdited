using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface IAdminPaymentService
    {
        Task<ApiResponse<PaymentSummaryDTO>> GetPaymentSummaryAsync();
        Task<ApiResponse<List<AdminPaymentDetailsDTO>>> GetPaymentDetails();
        Task<ApiResponse<ReviewGetDTOStudent>> GetReviewByBookingId(Guid bookingId);
        Task<ApiResponse<bool>> GetUpdatePaymentIntoCouncelor(Guid bookingId);
    }
}
