using Application.DTO;
using Application.Interface.Service;
using Domain.Entities;
using Infrastructure.Services.BookinService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexora.Controllers.BaseControllerClass;

namespace Nexora.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingByCouncelorController : BaseController
    {
        private readonly IBookingServiceByCouncelor _bookingServiceByCouncelor;
        private readonly ICouncelorService _councelorService;
        public BookingByCouncelorController(IBookingServiceByCouncelor bookingServiceByCouncelor,ICouncelorService councelorService)
        {
            _bookingServiceByCouncelor = bookingServiceByCouncelor;
            _councelorService = councelorService;
        }
        [HttpGet("councelor-get-request")]
        public async Task<IActionResult> GetRequestForBooking()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);

            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }

            var result = await _bookingServiceByCouncelor.GetPendingRequestByCounselorId(counselorId.Value);

            if (result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("councelor-bookings-reshedule")]
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
        public async Task<IActionResult> GetCancelledBooking()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }

            var result = await _bookingServiceByCouncelor.GetCancelledByCounselorId (counselorId.Value);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-Confirmed-bookings")]
        public async Task<IActionResult> GetConfirmedBookings()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }
            var result = await _bookingServiceByCouncelor.GetConfirmedBookings (counselorId.Value);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("councelor-get-completed-bookings")]
        public async Task<IActionResult> GetcompletedBookings()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }
            var result = await _bookingServiceByCouncelor.GetcompletedBookings (counselorId.Value);
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
        [HttpGet("councelor-get-id")]
        public async Task<IActionResult> GetCouncelorId()
        {
            var userId = GetLoggedInUserId().Value;
            var result = await _councelorService.getCounseloridByUserId(userId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
