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
    public class CouncelorController : BaseController
    {

        private readonly ICouncelorService _councelorService;
        public CouncelorController(ICouncelorService councelorService)
        {
            _councelorService = councelorService;
        }
       
        [HttpGet("Councelor-By-Id")]
        public async Task<IActionResult> GetCouncelorById(Guid CouncellorId)
        {
            var result = await _councelorService.GetCounselorById(CouncellorId);
            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet("all-Councelor")]
        public async Task<IActionResult> GetAllCouncelor()
        {
            var result = await _councelorService.GetAllCounselorsAsync ();
            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("all-CouncelorByKeyword")]
        public async Task<IActionResult> GetCouncelorByKeyword(string keyword)
        {
            var result = await _councelorService.GetCounselorsByKeywords(keyword);
            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddEducation([FromForm] EducationCreateDTO dto)
        {
            var response = await _councelorService.AddEducationAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("get/education/id")]
        public async Task<IActionResult> GetEducation(Guid id)
        {
            var response = await _councelorService.GetEducationByCounselorIdAsync(id);
            if (response.StatusCode == 200)
                return Ok(response);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("CheckStatus")]
        public async Task<IActionResult> CheckStatus()
        {
             var userId = GetLoggedInUserId().Value;
             var result = await _councelorService.GetCounselorStatusAsync(userId);

            return Ok(new
            {
                statusCode = 200,
                message = "Counselor status fetched",
                data = result,
                error = (string)null
            });
        }
    }
}
