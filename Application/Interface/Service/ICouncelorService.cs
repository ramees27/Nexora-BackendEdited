using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface ICouncelorService
    {
        Task<ApiResponse<object>> ApplyAsCounselor(CounselorAddDTO dto,Guid userId);
        Task<ApiResponse<CouncellorGetDTO>> GetCounselorById(Guid counselorId);
        Task<ApiResponse<List<CouncellorGetDTO>>> GetAllCounselorsAsync();
        Task<ApiResponse<List<CouncellorGetDTO>>> GetCounselorsByKeywords(string keyword);
        Task<ApiResponse<object>> AddEducationAsync(EducationCreateDTO dto);
    }
}
