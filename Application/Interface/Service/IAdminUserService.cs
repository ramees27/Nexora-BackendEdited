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
    public interface IAdminUserService
    {
        Task<ApiResponse<List<UserDetailsDTO>>> GetAllStudentsForUserManagement();
        Task<ApiResponse<List<CounselorDetailsDTO>>> GetverifiedCounselorsAsync();
        
        Task<ApiResponse<bool>> SoftDeleteCounselor(Guid id);
        Task<ApiResponse<bool>> SoftDeleteStudents(Guid id);

    }
}
