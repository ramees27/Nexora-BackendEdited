using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Application.Interface.Service;
using Domain;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.AdminService
{
    public class AdminPaymentService:IAdminPaymentService
    {
        private readonly IAdminPaymentRepository _repository;
        private readonly ILogger <AdminPaymentService> _logger;
        public AdminPaymentService(IAdminPaymentRepository repository, ILogger<AdminPaymentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<ApiResponse<PaymentSummaryDTO>> GetPaymentSummaryAsync()
        {
            try
            {
                var summary = await _repository .GetPaymentSummaryAsync();

                return new ApiResponse<PaymentSummaryDTO>
                {
                    StatusCode = 200,
                    Message = "Payment summary fetched successfully",
                    Data = summary
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching payment summary.");
                return new ApiResponse<PaymentSummaryDTO>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching the summary",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<AdminPaymentDetailsDTO>>> GetPaymentDetails()
        {
            try
            {
                var result = await _repository.GetPaymentDetailsAsync();
                if (result == null)
                {
                    return new ApiResponse<List<AdminPaymentDetailsDTO>>
                    {
                        StatusCode = 404,
                        Message = "No Payment Details found",
                        Data = null
                    };
                }
                return new ApiResponse<List<AdminPaymentDetailsDTO>>
                {
                    StatusCode = 200,
                    Message = "Payment Details Fetched Succussfully",
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return new ApiResponse<List<AdminPaymentDetailsDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occured",
                    Data = null
                };
            }
            }
        public async Task<ApiResponse<ReviewGetDTOStudent>> GetReviewByBookingId(Guid bookingId)
        {
            try
            {
                var review = await _repository.GetReviewByBookingIdAsync(bookingId);
                if (review == null)
                {
                    return new ApiResponse<ReviewGetDTOStudent>
                    {
                        StatusCode = 404,
                        Message = "Review not found",
                        Data = null
                    };
                }

                return new ApiResponse<ReviewGetDTOStudent>
                {
                    StatusCode = 200,
                    Message = "Review fetched successfully",
                    Data = review
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the review");
                return new ApiResponse<ReviewGetDTOStudent>
                {
                    StatusCode = 500,
                    Message = "An error occurred",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<bool>> GetUpdatePaymentIntoCouncelor(Guid bookingId)
        {
            try
            {
                var result = await _repository.GetUpdatePaymentIntoCouncelor(bookingId);

                if (result == false)
                {
                    return new ApiResponse<bool>
                    {
                        StatusCode = 500,
                        Message = "Error",
                        Data = false

                    };
                }
                return new ApiResponse<bool>
                {
                    StatusCode = 200,
                    Message = "Payment Updated successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the payments");
                return new ApiResponse<bool>
                {
                    StatusCode = 500,
                    Message = "An error occurred",
                  
                };
            }

        }
    }
}

