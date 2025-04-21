using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public DashBoardController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("Get-Payemnt-Summery")]
        public async Task<IActionResult> GetPaymentDetailsforAdmin()
        {
          
            
                var result = await _adminService.GetPaymentSummeryForAdmin();
                if (result.StatusCode == 200)
                {
                    return Ok(result);
                }
                return StatusCode(result.StatusCode, result);
         }
        [HttpGet("Get-Councelor-Count")]
        public async Task<IActionResult> GetCouncelorCount()
               {


                  var result = await _adminService.GetVerifiedCounselorCountAsync();
                   if (result.StatusCode == 200)
                    {
                return Ok(result);
                    }
                  return StatusCode(result.StatusCode, result);
               }
        [HttpGet("Get-Student-Count")]
        public async Task<IActionResult> GetStudentCount()
        {


            var result = await _adminService.GetStudentCounts();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Get-Booking-Count")]
        public async Task<IActionResult> GetTotalBooking()
        {


            var result = await _adminService.GetTotalBookinCount();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Get-Status-Count")]
        public async Task<IActionResult> GetStatusCountofBookings()
        {


            var result = await _adminService.GetBookingStatusCounts();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}

