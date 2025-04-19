using Application.DTO;
using Application.Interface.Service;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexora.Controllers.BaseControllerClass;

namespace Nexora.Controllers.CouncelorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouncelorAuthController : BaseController
    {
        private readonly ICouncelorService _councelorService;
        public CouncelorAuthController (ICouncelorService councelorService)
        {
            _councelorService = councelorService;
        }
        [Authorize]
        [HttpPost("Apply-Councelor")]
        public async Task<IActionResult> ApplyConucelor(CounselorAddDTO councelorAddDTO)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = 401,
                    Message = "Unauthorized"
                });
            }
           
            var result=await _councelorService.ApplyAsCounselor(councelorAddDTO,userId.Value);
            return StatusCode(result.StatusCode, result);

        }
    }
}
