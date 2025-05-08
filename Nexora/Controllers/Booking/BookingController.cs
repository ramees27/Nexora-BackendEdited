using Application.DTO;
using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Nexora.Controllers.BaseControllerClass;

namespace Nexora.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("Add-Booking")]
        public async Task<IActionResult> AddBooking(BookingRequestDto bookingRequestDto, Guid CounceloorId)
        {
            var userId = GetLoggedInUserId().Value;
            var result = await _bookingService.BookingRequest(bookingRequestDto, userId, CounceloorId);
            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("{bookingId}/status/{status}")]
        public async Task<IActionResult> UpdateBookingStatus(Guid bookingId, string status)
        {

            var result = await _bookingService.UpdateStatusAsync(bookingId, status);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);

        }

        [HttpGet("student/pending")]
        public async Task<IActionResult> GetPendingRequestPaymentBookings()
        {
            var studentId = GetLoggedInUserId().Value;
            var result = await _bookingService.GetPendingRequestPaymentBookings(studentId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> UpdatePaymentAndAccepct(Guid bookingId)
        {
           
            var result = await _bookingService.UpdatePaymentAndStatusAsync(bookingId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("student-Get-ScheduledBookings")]
        public async Task<IActionResult> GetScheduledBookings()
        {
            var studentId = GetLoggedInUserId().Value;
            var result = await _bookingService.GetBookingsByScheduled (studentId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("student-Get-completed-Bookings")]
        public async Task<IActionResult> GetCompletedBookings()
        {
            var studentId = GetLoggedInUserId().Value;
            var result = await _bookingService.GetCompletedBookings(studentId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("student-Get-Cancelled-Declained-Bookings")]
        public async Task<IActionResult> GetCancelledandDeclainedBookings()
        {
            var studentId = GetLoggedInUserId().Value;
            var result = await _bookingService.GetCancelledorRejectedBookings (studentId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
