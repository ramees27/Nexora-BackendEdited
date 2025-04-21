using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Interface.Repository
{
    public interface IAdminRepository
    {
        Task<AdminPaymentDTO> GetPaymentDetailsForAdmin();
        Task<int> GetActiveVerifiedCounselorCountAsync();
        Task<int> GetStudentCount();
        Task<int> GetTotalBookinCount();
        Task<List<StatusCountDto>> GetBookingStatusCountsAsync();
    }
}
