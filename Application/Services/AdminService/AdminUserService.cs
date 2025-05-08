using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Service;
using Domain.Entities;
using Domain;
using Microsoft.Extensions.Logging;
using Application.Interface.Repository;
using Application.DTO;

namespace Infrastructure.Services.AdminService
{
    public class AdminUserService:IAdminUserService
    {
        private readonly IAdminUserRepository _IAdminUserRepository;
        private readonly ILogger<AdminUserService> _logger;
        public AdminUserService(IAdminUserRepository adminUserService, ILogger<AdminUserService> logger)
        {
            _IAdminUserRepository = adminUserService;
            _logger=logger;

         }
        public async Task<ApiResponse<List<UserDetailsDTO>>> GetAllStudentsForUserManagement()
        {
            try
            {
                var students = await _IAdminUserRepository.GetAllStudentsForUserManagementAsync();

                return new ApiResponse<List<UserDetailsDTO>>
                {
                    StatusCode = 200,
                    Message = "Students fetched successfully",
                    Data = students
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching students for user management.");
                return new ApiResponse<List<UserDetailsDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving students",
                    Data = null
                };
            }
        
        }
        public async Task<ApiResponse<List<CounselorDetailsDTO>>> GetverifiedCounselorsAsync()
        {
            try
            {
                var counselors = await _IAdminUserRepository.GetverifiedCounselorsAsync();

                return new ApiResponse<List<CounselorDetailsDTO>>
                {
                    StatusCode = 200,
                    Message = "verified counselors fetched successfully",
                    Data = counselors
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching verified counselors.");
                return new ApiResponse<List<CounselorDetailsDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<bool>> SoftDeleteCounselor(Guid id)
        {
            var deleted = await _IAdminUserRepository.SoftDeleteCounselorAsync(id);
            if (!deleted)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = 404,
                    Message = "Counselor not found or already deleted",
                    Data = deleted
                };
            }

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Message = "Deleted Suucuusfully",
                Data = deleted
            };
        }
        public async Task<ApiResponse<bool>> SoftDeleteStudents(Guid id)
        {
            var deleted = await _IAdminUserRepository.SoftDeleteStudentAsync(id);
            if (!deleted)
            {
                return new ApiResponse<bool>
                {
                    StatusCode = 404,
                    Message = "Student not found or already deleted",
                    Data = deleted
                };
            }

            return new ApiResponse<bool>
            {
                StatusCode = 200,
                Message = "Deleted Succussfully",
                Data = deleted
            };
        }


    }
}
