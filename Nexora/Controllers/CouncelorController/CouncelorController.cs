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
        [Authorize]
        [HttpGet("Councelor-By-Id")]
        public async Task<IActionResult> GetCouncelorById(Guid CouncellorId)
        {
            var result = await _councelorService.GetCounselorById(CouncellorId);
            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
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
    }
}
