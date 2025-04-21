using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interface.Repository
{
    public interface IBookinRepositiryByCouncelor
    {
       
        Task<List<BookingGetDTOByCouncelor>> GetPendingAndRequestedPaymentBookingsByCounselorId(Guid counselorId);
        Task<bool> RescheduleBookingAsync(BookingRescheduleDTO dto);
        Task<List<BookingGetDTOByCouncelor>> GetCancelledBookingsByCounselorId(Guid counselorId);
        Task<List<BookingGetDTOByCouncelor>> GetUpcomingConfirmedBookings(Guid counselorId);
        Task<List<BookingGetDTOByCouncelor>> GetUpcompletedBookings(Guid counselorId);
        Task<BookingSectionDetailsDTO> GetBookingSectionDetailsById(Guid bookingId);

    }
}
