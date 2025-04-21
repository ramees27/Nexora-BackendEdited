using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.Controllers.CouncelorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    { private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("Get-Payment")]
        public async Task<IActionResult>GetPayments(Guid councelorId)
        {
            var result=await _paymentService.GetPaymentWithBooking(councelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("Get-Councelor-paymentdetails")]
        public async Task<IActionResult> GetPaymentDetails(Guid councelorId)
        {
            var result = await _paymentService.GetPaymentDetails (councelorId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
