using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface IBookingRepository
    {
        Task<int> AddBookingAsync(Booking booking, Guid counselor_id);
        Task<bool> UpdateBookingStatusAsync(Guid bookingId, string status);
     
        Task<List<ActivityGetDTOForUser>> GetPendingRequestPayemntBookings(Guid studentId);
        Task<bool> UpdateBookingPaymentAndStatusAsync(Guid bookingId);
        Task<List<ActivityGetDTOForUser>> GetScheduledBookings(Guid studentId);
        Task<List<ActivityGetDTOForUser>> GetCompletedBookings(Guid studentId);
        Task<List<ActivityGetDTOForUser>> GetRejectedandCancelledBookings(Guid studentId);
      

    }
}
