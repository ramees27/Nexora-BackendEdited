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
    public interface IAdminService
    {
        Task<ApiResponse<AdminPaymentDTO>> GetPaymentSummeryForAdmin();
        Task<ApiResponse<int>> GetVerifiedCounselorCountAsync();
        Task<ApiResponse<int>> GetStudentCounts();
        Task<ApiResponse<int>> GetTotalBookinCount();
        Task <ApiResponse<List<StatusCountDto>>> GetBookingStatusCounts();
        Task<ApiResponse<int>> GetNonVerifiedCounselorCount();
        Task<ApiResponse<List<CounselorDetailsDTO>>> GetNewCounselorsForAdminAsync();
        Task<ApiResponse<object>> VerifyCounselor(Guid counselorId);
        Task<ApiResponse<object>> DeleteCounselorApplicationAsync(Guid counselorId);
        Task<ApiResponse<List<BookinGetAdminDTO>>> GetAllBookingDetails();
        Task<ApiResponse<List<MonthlyIncomeExpenseDto>>> GetMonthlyIncomeExpenseAsync();
    }
}
