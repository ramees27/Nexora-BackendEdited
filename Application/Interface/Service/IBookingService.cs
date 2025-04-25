using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;
using Domain.Entities;

namespace Application.Interface.Service
{
    public interface IBookingService
    {
        Task<ApiResponse<Guid>> BookingRequest(BookingRequestDto request, Guid userId, Guid Councelor_id);
        Task<ApiResponse<string>> UpdateStatusAsync(Guid bookingId, string status);


        Task<ApiResponse<List<ActivityGetDTOForUser>>> GetPendingRequestPaymentBookings(Guid studentId);
        Task<ApiResponse<string>> UpdatePaymentAndStatusAsync(Guid bookingId);
        Task<ApiResponse<List<ActivityGetDTOForUser>>> GetBookingsByScheduled(Guid studentId);
        Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCompletedBookings(Guid studentId);
        Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCancelledorRejectedBookings(Guid studentId);
    }
}
