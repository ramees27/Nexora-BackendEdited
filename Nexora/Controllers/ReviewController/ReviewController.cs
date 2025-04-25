using Application.DTO;
using Application.Interface.Service;
using Domain.Entities;
using Infrastructure.Repository;
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
        private readonly INotificationRepository _notificationRepository;

        public ReviewController(IReviewService reviewService,INotificationRepository notificationRepository)
        {
            _reviewService = reviewService;
            _notificationRepository = notificationRepository;
            
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
        [HttpGet("Get-Review-Councelorid")]
        public async Task<IActionResult> GetReviewByCouncelorId(Guid Councelor_id)
        {
           
            var result = await _reviewService.GetReviewsByCouncelorIdForStudents(Councelor_id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Get-Average-Rating")]
        public async Task<IActionResult> GetAverageReview(Guid Councelor_id)
        {

            var result = await _reviewService.GetReviewAverageRating (Councelor_id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("Get-all-review")]
        public async Task<IActionResult> GetAllReview()
        {

            var result = await _reviewService.GetAllReviews();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
      

    }
}
