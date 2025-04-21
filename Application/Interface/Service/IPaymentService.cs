using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface IPaymentService
    {
        Task<ApiResponse<List<PaymentWithBookingDTO>>> GetPaymentWithBooking(Guid Councelorid);
        Task<ApiResponse<PaymentSummaryDTO>> GetPaymentDetails(Guid counselorId);
    }
}

