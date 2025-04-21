using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface IAdminService
    {
        Task<ApiResponse<AdminPaymentDTO>> GetPaymentSummeryForAdmin();
        Task<ApiResponse<int>> GetVerifiedCounselorCountAsync();
        Task<ApiResponse<int>> GetStudentCounts();
        Task<ApiResponse<int>> GetTotalBookinCount();
        Task <ApiResponse<List<StatusCountDto>>> GetBookingStatusCounts();
    }
}
