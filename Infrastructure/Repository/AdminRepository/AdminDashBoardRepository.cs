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

namespace Infrastructure.Repository.AdminRepository
{
    public class AdminDashBoardRepository : IAdminRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminDashBoardRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<AdminPaymentDTO> GetPaymentDetailsForAdmin()
        {
            var sql = @"SELECT SUM(total_amount) AS total_amount,
                SUM(counselor_amount) AS counselor_amount,
                SUM(commission_amount) AS commission_amount
                FROM payments;";
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<AdminPaymentDTO>(sql);
            return result;
        }
        public async Task<int> GetActiveVerifiedCounselorCountAsync()
        {
            var sql = @"SELECT COUNT(*) 
                FROM counselors 
                WHERE is_verified = TRUE AND is_deleted = FALSE";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QuerySingleAsync<int>(sql);

            return result;
        }
        public async Task<int> GetStudentCount()
        {

            var sql = "SELECT COUNT(*) FROM students s WHERE s.is_deleted = FALSE AND s.student_id NOT IN( SELECT c.user_id FROM counselors c WHERE c.is_verified = TRUE)";

            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleAsync<int>(sql);


        }
        public async Task<int> GetTotalBookinCount()
        {

            var sql = @"SELECT COUNT(*)
                    FROM bookings ";

            using var connection = _dapperContext.CreateConnection();
            return await connection.QuerySingleAsync<int>(sql);


        }
        public async Task<List<StatusCountDto>> GetBookingStatusCountsAsync()
        {
            var sql = @"
            SELECT s.status, COUNT(b.status) AS count
            FROM (
                SELECT 'pending' AS status
                UNION ALL SELECT 'request_payment'
                UNION ALL SELECT 'accepted'
                UNION ALL SELECT 'declined'
                UNION ALL SELECT 'cancelled'
                UNION ALL SELECT 'completed'
            ) s
            LEFT JOIN bookings b ON b.status = s.status
            GROUP BY s.status;";


            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<StatusCountDto>(sql);
            return result.ToList();
        }
        public async Task<int> GetNonVerifiedCounselorCountAsync()
        {
            var sql = @"SELECT COUNT(*) 
                FROM counselors 
                WHERE is_verified = False AND is_deleted = FALSE";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QuerySingleAsync<int>(sql);

            return result;
        }
        public async Task<List<CounselorDetailsDTO>> GetNewCounselorsForAdminAsync()
        {
            var sql = @"SELECT * FROM counselors c LEFT JOIN education e ON c.counselors_id = e.counselor_id
                         WHERE c.is_verified = FALSE AND c.is_deleted = FALSE;";
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<CounselorDetailsDTO>(sql);

            return result.ToList();

        }
        public async Task<bool> VerifyCounselorAsync(Guid counselorId)
        {
            var sql = @"UPDATE counselors 
                SET is_verified = TRUE 
                WHERE counselors_id = @CounselorId AND is_deleted = FALSE";

            using var connection = _dapperContext.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { CounselorId = counselorId });
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCounselorApplicationAsync(Guid counselorId)
        {
            var sql = @"DELETE FROM counselors WHERE counselors_id = @CounselorId AND is_verified = FALSE";

            
                using var connection = _dapperContext.CreateConnection();
                var rows = await connection.ExecuteAsync(sql, new { CounselorId = counselorId });
                return rows > 0;
           
        }
       


    }



}
