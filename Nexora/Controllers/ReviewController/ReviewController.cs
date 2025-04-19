using Application.DTO;
using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexora.Controllers.BaseControllerClass;

namespace Nexora.Controllers.ReviewController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost("Add-Review")]
        public async Task<IActionResult>AddReview(ReviewAddDTO reviewAddDTO,Guid councelor_id,Guid booking_id)
        {
            var userId=GetLoggedInUserId().Value;
            var result = await _reviewService.AddReviews(reviewAddDTO, userId, councelor_id, booking_id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

    }
}
