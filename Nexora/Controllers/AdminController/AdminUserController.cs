using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }
       
        [HttpGet("admin/students")]
        public async Task<IActionResult> GetStudentsForUserManagement()
        {
            var response = await _adminUserService.GetAllStudentsForUserManagement();
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("admin/Councelors")]
        public async Task<IActionResult> GetCouncelorsForUserManagement()
        {
            var response = await _adminUserService.GetverifiedCounselorsAsync();
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpPatch("admin/stdents-delete")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var response = await _adminUserService.SoftDeleteStudents(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpPatch("admin/concelors-delete")]
        public async Task<IActionResult> Deleteconcelors(Guid id)
        {
            var response = await _adminUserService.SoftDeleteCounselor(id);
            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            return StatusCode(response.StatusCode, response);
        }
    }
}
