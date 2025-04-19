using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Nexora.Controllers.BaseControllerClass
{
    public  class BaseController : ControllerBase
    {
        protected Guid? GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
            return null;
        }
    }
}