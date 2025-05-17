using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexora.Controllers.BaseControllerClass;

namespace Nexora.Controllers.CouncelorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    { private readonly IPaymentService _paymentService;
        private readonly ICouncelorService _councelorService;
        public PaymentController(IPaymentService paymentService, ICouncelorService councelorService)
        {
            _paymentService = paymentService;
            _councelorService = councelorService;
        }
        [HttpGet("Get-Payment")]
        public async Task<IActionResult>GetPayments()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }
            var result=await _paymentService.GetPaymentWithBooking(counselorId.Value);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("Get-Councelor-paymentdetails")]
        public async Task<IActionResult> GetPaymentDetails()
        {
            var userId = GetLoggedInUserId().Value;
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }
            var result = await _paymentService.GetPaymentDetails (counselorId.Value);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
