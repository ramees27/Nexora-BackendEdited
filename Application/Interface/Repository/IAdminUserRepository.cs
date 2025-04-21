using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface IAdminUserRepository
    {
        Task<List<UserDetailsDTO>> GetAllStudentsForUserManagementAsync();
        Task<List<CounselorDetailsDTO>> GetverifiedCounselorsAsync();
        Task<bool> SoftDeleteStudentAsync(Guid id);
        Task<bool> SoftDeleteCounselorAsync(Guid id);
    }
}
