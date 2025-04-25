using Application.DTO;
using Application.Interface.Service;
using Infrastructure.Services.BookinService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingByCouncelorController : ControllerBase
    {
        private readonly IBookingServiceByCouncelor _bookingServiceByCouncelor;
        public BookingByCouncelorController(IBookingServiceByCouncelor bookingServiceByCouncelor)
        {
            _bookingServiceByCouncelor = bookingServiceByCouncelor;
        }
        [HttpGet("councelor-get-request")]
        public async Task<IActionResult> GetRequestForBooking(Guid CouncelorId)
        {
            var result =await _bookingServiceByCouncelor.GetPendingRequestByCounselorId(CouncelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode,result);
        }
        [HttpPut("councelor-bookings-reshedule")]
        public async Task<IActionResult> UpdateBookingDateandtime(BookingRescheduleDTO bookingresheduleDto)
        {
            var result = await _bookingServiceByCouncelor.RescheduleBookingAsync (bookingresheduleDto);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-Cancelled-bookings")]
        public async Task<IActionResult> GetCancelledBooking(Guid CouncelorId)
        {
            var result = await _bookingServiceByCouncelor.GetCancelledByCounselorId (CouncelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-Confirmed-bookings")]
        public async Task<IActionResult> GetConfirmedBookings(Guid CouncelorId)
        {
            var result = await _bookingServiceByCouncelor.GetConfirmedBookings (CouncelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-completed-bookings")]
        public async Task<IActionResult> GetcompletedBookings(Guid CouncelorId)
        {
            var result = await _bookingServiceByCouncelor.GetcompletedBookings (CouncelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-Details-Completed-bookings")]
        public async Task<IActionResult> GetcompletedBookingsDetails(Guid bookingId)
        {
            var result = await _bookingServiceByCouncelor.GetBookingSectionDetails (bookingId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("{bookingId}/:status/{status}")]
        public async Task<IActionResult> UpdateBookingStatus(Guid bookingId, string status)
        {

            var result = await _bookingServiceByCouncelor.UpdateStatusByCouncelor(bookingId, status);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);

        }
    }
}
