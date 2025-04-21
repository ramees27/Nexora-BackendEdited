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

    }
}
