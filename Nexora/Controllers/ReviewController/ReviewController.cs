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
        private readonly ICouncelorService _councelorService;

        public ReviewController(IReviewService reviewService,INotificationRepository notificationRepository,ICouncelorService councelorService)
        {
            _reviewService = reviewService;
            _notificationRepository = notificationRepository;
            _councelorService=councelorService;
            
        }
        [HttpPost("Add-Review")]
        public async Task<IActionResult>AddReview(ReviewAddDTO reviewAddDTO)
        {
            var userId=GetLoggedInUserId().Value;
            var result = await _reviewService.AddReviews(reviewAddDTO, userId);
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
        [HttpGet("Get/Review/by/ConcelorId")]
        public async Task<IActionResult> GetReviewByCouncelorIdforCouncelors()
        {
            var userIdNullable = GetLoggedInUserId();
            if (userIdNullable == null)
            {
                return Ok(null);
            }

            var userId = userIdNullable.Value;

            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }

            var result = await _reviewService.GetReviewsByCouncelorIdForStudents(counselorId.Value);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Get-Average-Rating")]
        public async Task<IActionResult?> GetAverageReview()
        {
            var userIdNullable = GetLoggedInUserId();
            if (userIdNullable == null)
            {
                return Ok(null);
            }

            var userId = userIdNullable.Value;
           
            var counselorIdResponse = await _councelorService.getCounseloridByUserId(userId);
            var counselorId = counselorIdResponse.Data as Guid?;

            if (counselorId == null)
            {
                return Ok(null);
            }
            Console.WriteLine($"{counselorId}/n  /n /n /n /n");


            var result = await _reviewService.GetReviewAverageRating(counselorId.Value);

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
        [HttpGet("Check-Review-Exists/{bookingId}")]
        public async Task<IActionResult> CheckReviewExists(Guid bookingId)
        {

            var result = await _reviewService.IsRatingExistsAsync(bookingId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("get/review/{bookingId}")]
        public async Task<IActionResult> GetReviewByBookingId(Guid bookingId)
        {

            var result = await _reviewService.GetReviewByBookingId(bookingId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

    }
}
