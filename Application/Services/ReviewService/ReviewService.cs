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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICouncelorRepo _councelorRepo;
        public ReviewService(IMapper mapper, ILogger<ReviewService> logger, IReviewRepository reviewRepository, ICouncelorRepo councelorRepo)
        {
            _mapper = mapper;
            _logger = logger;
            _reviewRepository = reviewRepository;
            _councelorRepo = councelorRepo;
        }

        public async Task<ApiResponse<int>> AddReviews(ReviewAddDTO reviewAddDTO, Guid UserId)
        {
            try
            {
                var review = _mapper.Map<Review>(reviewAddDTO);

                // Manually set fields not present in the DTO
                review.student_id = UserId;
                review.counselor_id = reviewAddDTO.counselor_id;
                review.booking_id = reviewAddDTO.booking_id;
                _logger.LogInformation("Attempting to add review for Booking ID: {BookingId} by Student ID: {StudentId}", reviewAddDTO.booking_id, UserId);

                var result = await _reviewRepository.AddReview(review);

                if (result > 0)
                {
                    _logger.LogInformation("Successfully added review for Booking ID: {BookingId} by Student ID: {StudentId}", reviewAddDTO.booking_id, UserId);
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
                _logger.LogError(ex, "Error occurred while adding review for Booking ID: {BookingId} by Student ID: {StudentId}", reviewAddDTO.booking_id, UserId);
                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing the review"
                };
            }
        }
        public async Task<ApiResponse<List<ReviewGetDTOStudent>>> GetReviewsByCouncelorIdForStudents(Guid Councelor_id)
        {

            try
            {
                var isValid = await _councelorRepo.IsValidCounselor(Councelor_id);
                if (!isValid)
                {
                    return new ApiResponse<List<ReviewGetDTOStudent>>
                    {
                        StatusCode = 400,
                        Message = "Counselor is not verified or has been blocked."
                    };
                }

                var result = await _reviewRepository.GetReviewsByCouncelorId(Councelor_id);
                if (result == null)
                {
                    return new ApiResponse<List<ReviewGetDTOStudent>>
                    {
                        StatusCode = 200,
                        Message = "No Reviews Found for this Councelor",
                        Data = null
                    };
                }
                return new ApiResponse<List<ReviewGetDTOStudent>>
                {

                    StatusCode = 200,
                    Message = " Reviews Succusfully fetched",
                    Data = result,
                    Error = null

                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all reviews");
                return new ApiResponse<List<ReviewGetDTOStudent>>
                {
                    StatusCode = 500,
                    Message = "Error while fetching Reviews",
                    Data = null
                };
            }

        }
        public async Task<ApiResponse<AvrageRatingDTO?>> GetReviewAverageRating(Guid counselorId)
        {
            try

            {

                var result = await _reviewRepository.GetReviewCountAndAverageRating(counselorId);
                if (result == null)
                {
                    return new ApiResponse<AvrageRatingDTO?>
                    {
                        StatusCode = 200,
                        Message = "No reviews found for this counselor.",

                        Data= null
                    };
                }



                return new ApiResponse<AvrageRatingDTO?>
                {
                    StatusCode = 200,
                    Message = "Review count and average rating fetched successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Geting Average Review and ratings");
                return new ApiResponse<AvrageRatingDTO?>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing the review",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<ReviewGetDTOStudent>>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewRepository.GetAllReviewsAsync();

                return new ApiResponse<List<ReviewGetDTOStudent>>
                {
                    StatusCode = 200,
                    Message = "Reviews fetched successfully.",
                    Data = reviews
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching reviews.");
                return new ApiResponse<List<ReviewGetDTOStudent>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching reviews.",
                    Data = null
                };
            }

        }
        public async Task<ApiResponse<bool>> IsRatingExistsAsync(Guid bookingId)
        {
            var result= await _reviewRepository.IsRatingExistsAsync(bookingId);
            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Message = "Review Checked",
                Data = result
            };
        }
        public async Task<ApiResponse<Review> >GetReviewByBookingId(Guid bookingId)
        {
            try
            {
                var result = await _reviewRepository.GetReviewByBookingId(bookingId);
                if (result == null)
                {
                    return new ApiResponse<Review>
                    {
                        StatusCode = 200,
                        Message = "No review for this booking",
                        Data = null
                    };
                }
                return new ApiResponse<Review>
                {
                    StatusCode = 200,
                    Message = "Reviews",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Review>
                {
                    StatusCode = 500,
                    Message = ex.Message,

                };
            }

        }
    }
}
