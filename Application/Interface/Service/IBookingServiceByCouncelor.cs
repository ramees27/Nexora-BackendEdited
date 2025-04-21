using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface IBookingServiceByCouncelor
    {
        Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetPendingRequestByCounselorId(Guid counselorId);
        Task<ApiResponse<string>> RescheduleBookingAsync(BookingRescheduleDTO dto);
        Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetCancelledByCounselorId(Guid counselorId);
        Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetConfirmedBookings(Guid counselorId);
        Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetcompletedBookings(Guid counselorId);
        Task<ApiResponse<BookingSectionDetailsDTO>> GetBookingSectionDetails(Guid bookingId);
    }
}
