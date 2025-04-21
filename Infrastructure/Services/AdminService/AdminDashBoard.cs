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
    public class AdminDashBoardService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminDashBoardService> _logger;
        public AdminDashBoardService(IAdminRepository adminRepository, ILogger<AdminDashBoardService> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }
        public async Task<ApiResponse<AdminPaymentDTO> >GetPaymentSummeryForAdmin()
        {
            try
            {
                var result = await _adminRepository.GetPaymentDetailsForAdmin();
                return new ApiResponse<AdminPaymentDTO>
                {
                    StatusCode = 500,
                    Message = "Succuss",
                    Data = result,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While getting Payment Summer");
                return new ApiResponse<AdminPaymentDTO>
                {
                    StatusCode = 500,
                    Message = "Error While getting Payment Summer",
                    Data = null,

                };
            }
        }
        public async Task<ApiResponse<int>> GetVerifiedCounselorCountAsync()
        {
            try
            {
                var count = await _adminRepository.GetActiveVerifiedCounselorCountAsync();

                return new ApiResponse<int>
                {
                    StatusCode = 200,
                    Message = "Successfully fetched verified and active counselor count",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetVerifiedCounselorCountAsync");

                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving counselor count",
                    Data = 0
                };
            }
        }
        public async Task<ApiResponse<int>> GetStudentCounts()
        {
            try
            {
                var count = await _adminRepository.GetActiveVerifiedCounselorCountAsync();

                return new ApiResponse<int>
                {
                    StatusCode = 200,
                    Message = "Successfully fetched verified and active counselor count",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetVerifiedCounselorCountAsync");

                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving counselor count",
                    Data = 0
                };
            }
        }
        public async Task<ApiResponse<int>> GetTotalBookinCount()
        {
            try
            {
                var count = await _adminRepository.GetTotalBookinCount();

                return new ApiResponse<int>
                {
                    StatusCode = 200,
                    Message = "Successfully fetched Total Bookings count",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Total Bookings count");

                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while Total Bookings count",
                    Data = 0
                };
            }
        }
       public async Task<ApiResponse<List<StatusCountDto>>> GetBookingStatusCounts()
        {
            try
            {
                var result = await _adminRepository.GetBookingStatusCountsAsync();
                return new ApiResponse<List<StatusCountDto>>
                {
                    StatusCode = 200,
                    Message = "Booking status summary fetched",
                    Data = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in service while getting booking status counts.");
                return new ApiResponse<List<StatusCountDto>>
                {
                    StatusCode = 500,
                    Message = "Failed to fetch booking status data.",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<int> >GetNonVerifiedCounselorCount()
        {
            try
            {
                var count = await _adminRepository.GetNonVerifiedCounselorCountAsync();

                return new ApiResponse<int>
                {
                    StatusCode = 200,
                    Message = "Successfully fetched non-verified counselors count",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetVerifiedCounselorCountAsync");

                return new ApiResponse<int>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving counselor count",
                    Data = 0
                };
            }
        }
        public async Task<ApiResponse<List<CounselorDetailsDTO>>> GetNewCounselorsForAdminAsync()
        {
            try
            {
                var counselors = await _adminRepository.GetNewCounselorsForAdminAsync();

                if (counselors == null || !counselors.Any())
                {
                    _logger.LogInformation("No new counselors found for the admin.");
                    return new ApiResponse<List<CounselorDetailsDTO>>
                    {
                        StatusCode = 404,
                        Message = "No new counselors found.",
                        Data = null
                    };
                }

                _logger.LogInformation($"Retrieved {counselors.Count()} new counselors for admin.");
                return new ApiResponse<List<CounselorDetailsDTO>>
                {
                    StatusCode = 200,
                    Message = "Successfully retrieved new counselors.",
                    Data = counselors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching new counselors for admin.");
                return new ApiResponse<List<CounselorDetailsDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching counselor details.",
                    Data = null
                };
            }
        }
        public async  Task<ApiResponse<object>> VerifyCounselor(Guid counselorId)
        {
            try
            {
                var success = await _adminRepository.VerifyCounselorAsync(counselorId);

                if (!success)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = "Counselor not found or already verified/deleted",
                        Data = null
                    };
                }

                return new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Counselor verified successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying the counselor.");
                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "An error occurred",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<object>> DeleteCounselorApplicationAsync(Guid counselorId)
        {
            try
            {
                var deleted = await _adminRepository.DeleteCounselorApplicationAsync(counselorId);

                if (!deleted)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = "Counselor not found or already verified",
                        Data = null
                    };
                }

                return new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Counselor application deleted successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting counselor application.");
                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the counselor application",
                    Data = null
                };
            }
        }

    }
}
