using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface IAdminRepository
    {
        Task<AdminPaymentDTO> GetPaymentDetailsForAdmin();
        Task<int> GetActiveVerifiedCounselorCountAsync();
        Task<int> GetStudentCount();
        Task<int> GetTotalBookinCount();
        Task<List<StatusCountDto>> GetBookingStatusCountsAsync();
        Task<int> GetNonVerifiedCounselorCountAsync();
        Task<List<CounselorDetailsDTO>> GetNewCounselorsForAdminAsync();
        Task<bool> VerifyCounselorAsync(Guid counselorId);
        Task<bool> DeleteCounselorApplicationAsync(Guid counselorId);
        Task<List<BookinGetAdminDTO>> GetAllBookingDetails();
        Task<List<MonthlyIncomeExpenseDto>> GetMonthlyIncomeExpenseAsync();

    }
}
