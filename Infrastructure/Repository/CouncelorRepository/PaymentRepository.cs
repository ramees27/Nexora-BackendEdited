using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository.CouncelorRepository
{
    public  class PaymentRepository:IPaymentRepository
    {
        private readonly DapperContext _dapperContext;
        public PaymentRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<List<PaymentWithBookingDTO>> GetAllPaymentsWithBookingAsync(Guid councelorId)
        {
            var sql= @" SELECT * 
                     FROM payments p
                     JOIN bookings b ON p.booking_id = b.booking_id
                     JOIN users u ON p.UserId = u.UserId
                     WHERE p.counselors_id = @CounselorId";
            using var connection=_dapperContext.CreateConnection();
            var result = await connection.QueryAsync<PaymentWithBookingDTO>(sql, new { CounselorId = councelorId });
            return result.ToList();
        }
        public async Task<PaymentSummaryDTO> GetEarningsBreakdownAsync(Guid counselorId)
        {
            var sql = @"SELECT SUM(total_amount) AS TotalAmount,
                SUM(CASE WHEN admin_payout_status = 'pending' THEN counselor_amount ELSE 0 END) AS PendingAmount,
                SUM(CASE WHEN admin_payout_status = 'paid' THEN counselor_amount ELSE 0 END) AS ReceivedAmount
                FROM payments
                WHERE counselors_id = @CounselorId";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<PaymentSummaryDTO>(sql, new { CounselorId = counselorId });

            return result;
        }

    }
}
