using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;
using Domain.Entities;

namespace Application.Interface.Service
{
    public interface IReviewService
    {
        Task<ApiResponse<int>> AddReviews(ReviewAddDTO reviewAddDTO, Guid UserId);
        Task<ApiResponse<List<ReviewGetDTOStudent>>> GetReviewsByCouncelorIdForStudents(Guid Councelor_id);
        Task<ApiResponse<AvrageRatingDTO?> >GetReviewAverageRating(Guid counselorId);
        Task<ApiResponse<List<ReviewGetDTOStudent>>> GetAllReviews();
        Task<ApiResponse<bool>> IsRatingExistsAsync(Guid bookingId);
        Task<ApiResponse<Review>> GetReviewByBookingId(Guid bookingId);
    }
}
