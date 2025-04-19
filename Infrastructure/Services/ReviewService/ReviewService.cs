using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Application.Interface.Service;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ReviewService
{
    public class ReviewService:IReviewService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IMapper mapper, ILogger<ReviewService> logger, IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _reviewRepository = reviewRepository;
        }

        public async Task<ApiResponse<int>> AddReviews(ReviewAddDTO reviewAddDTO, Guid UserId, Guid councelor_id,Guid booking_id)
        {
            try
            {
                var review = _mapper.Map<Review>(reviewAddDTO);

                // Manually set fields not present in the DTO
                review.student_id = UserId;
                review.counselor_id = councelor_id;
                review.booking_id = booking_id;
                _logger.LogInformation("Attempting to add review for Booking ID: {BookingId} by Student ID: {StudentId}", booking_id, UserId);

                var result = await _reviewRepository.AddReview(review);

                if (result > 0)
                {
                    _logger.LogInformation("Successfully added review for Booking ID: {BookingId} by Student ID: {StudentId}", booking_id, UserId);
                    return new ApiResponse<int>
                    {
                        StatusCode = 200,
                        Message = "Review Added Successfully",
                        Data = result

                    };
                }
                return new ApiResponse<int>
                {
                    StatusCode = 400,
                    Message = "Failed to add review"
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding review for Booking ID: {BookingId} by Student ID: {StudentId}", booking_id, UserId);
                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing the review"
                };
            }
        }
       
    }
}
