using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Dapper;
using Infrastructure.Data;

namespace Infrastructure.Repository.AdminRepository
{
    public class AdminPaymentRepository : IAdminPaymentRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminPaymentRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<PaymentSummaryDTO> GetPaymentSummaryAsync()
        {


            var sql = @"
           SELECT 
  SUM(CASE 
        WHEN p.admin_payout_status = 'paid' 
        THEN p.counselor_amount 
        ELSE 0 
      END) AS PaidAmount,

  SUM(CASE 
        WHEN p.admin_payout_status = 'pending' 
             AND b.payment_status = true 
             AND (b.status = 'cancelled' OR b.status = 'completed') 
        THEN p.counselor_amount 
        ELSE 0 
      END) AS PendingAmount,

  SUM(p.counselor_amount) AS TotalAmount,
Sum(p.commission_amount) As CommisionAmout

FROM payments p
JOIN bookings b ON p.booking_id = b.booking_id;

";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<PaymentSummaryDTO>(sql);
            return result;
        }
        public async Task<List<AdminPaymentDetailsDTO>> GetPaymentDetailsAsync()
        {
            var Sql = @"SELECT  p.booking_id, u.username AS student_username,  c.full_name AS counselor_name,
                                 c.upi_id, b.preferd_date, b.preferd_time,b.status,b.payment_status, b.status AS booking_status, b.is_active,
                                 p.admin_payout_status, p.counselor_amount,  p.total_amount,  p.commission_amount,
                                 p.paidByStudent,  p.paid_on FROM payments p
                                JOIN bookings b ON p.booking_id = b.booking_id
                                JOIN users u ON b.student_id = u.UserId
                                JOIN counselors c ON b.counselor_id = c.counselors_id;";
            var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<AdminPaymentDetailsDTO>(Sql);
            return result.ToList();
        }
        public async Task<ReviewGetDTOStudent> GetReviewByBookingIdAsync(Guid bookingId)
        {
            var sql = @"SELECT * FROM ratings WHERE booking_id = @BookingId";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<ReviewGetDTOStudent>(sql, new { BookingId = bookingId });
            return result;
        
        }
        public async Task<bool> GetUpdatePaymentIntoCouncelor(Guid bookingId)
        {
            var sql = @"UPDATE payments 
                SET admin_payout_status = 'paid', 
                    paid_on = CURRENT_TIMESTAMP 
                WHERE booking_id = '@BookingId';";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { BookingId = bookingId });
            return result>0;

        }


    }
}
