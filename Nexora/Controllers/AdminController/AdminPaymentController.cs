using Application.Interface.Service;
using Infrastructure.Services.CouncelorService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPaymentController : ControllerBase
    {
        private readonly IAdminPaymentService _adminPaymentService;
        public AdminPaymentController(IAdminPaymentService adminPaymentService)
        {
            _adminPaymentService = adminPaymentService;
        }
        [HttpGet("payment-summary")]
        public async Task<IActionResult> GetPaymentSummary()
        {
            var response = await _adminPaymentService.GetPaymentSummaryAsync();
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("payment-Details")]
        public async Task<IActionResult> GetPaymentDetails()
        {
            var response = await _adminPaymentService.GetPaymentDetails();
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("Review-Details$bookinid")]
        public async Task<IActionResult> GetReviewDetails(Guid booking_id)
        {
            var response = await _adminPaymentService.GetReviewByBookingId(booking_id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpPatch("Review-payment-update")]
        public async Task<IActionResult> UpdatePayment(Guid booking_id)
        {
            var response = await _adminPaymentService.GetUpdatePaymentIntoCouncelor(booking_id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
    }
}
