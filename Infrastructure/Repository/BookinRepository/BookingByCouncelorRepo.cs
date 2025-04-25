using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository.BookinRepository
{
    public  class BookingByCouncelorRepo:IBookinRepositiryByCouncelor
    {
        private readonly DapperContext _dapperContext;
        public BookingByCouncelorRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<List<BookingGetDTOByCouncelor>> GetPendingAndRequestedPaymentBookingsByCounselorId(Guid counselorId)
        {
            var sql= @"SELECT *  FROM bookings b INNER JOIN users u ON b.student_id = u.UserId
        WHERE b.counselor_id = @CounselorId
        AND (b.status = 'pending' OR b.status = 'requested_payment')
        ORDER BY b.created_at DESC";
       
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<BookingGetDTOByCouncelor>(sql, new { CounselorId = counselorId });
            return result.ToList();
        }
        public async Task<bool> RescheduleBookingAsync(BookingRescheduleDTO dto)
        {
            var sql = @"UPDATE bookings 
                SET preferd_time = @PreferdTime, 
                    preferd_date = @PreferdDate 
                WHERE booking_id = @BookingId";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteAsync(sql, dto);
            return result > 0;
        }
        public async Task<List<BookingGetDTOByCouncelor>> GetCancelledBookingsByCounselorId(Guid counselorId)
        {
            var sql = @"SELECT *  FROM bookings b INNER JOIN users u ON b.student_id = u.UserId
        WHERE b.counselor_id = @CounselorId
        AND (b.status = 'cancelled' )
        ORDER BY b.created_at DESC";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<BookingGetDTOByCouncelor>(sql, new { CounselorId = counselorId });
            return result.ToList();
        }
        public async Task<List<BookingGetDTOByCouncelor>> GetUpcomingConfirmedBookings(Guid counselorId)
        {
            var sql = @"SELECT *  FROM bookings b INNER JOIN users u ON b.student_id = u.UserId
        WHERE b.counselor_id = @CounselorId
        AND (b.status = 'accepted' )
        ORDER BY b.created_at DESC";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<BookingGetDTOByCouncelor>(sql, new { CounselorId = counselorId });
            return result.ToList();
        }
        public async Task<List<BookingGetDTOByCouncelor>> GetUpcompletedBookings(Guid counselorId)
        {
            var sql = @"SELECT *  FROM bookings b INNER JOIN users u ON b.student_id = u.UserId
        WHERE b.counselor_id = @CounselorId
        AND (b.status = 'completed' )
        ORDER BY b.created_at DESC";

            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<BookingGetDTOByCouncelor>(sql, new { CounselorId = counselorId });
            return result.ToList();
        }
        public async Task<BookingSectionDetailsDTO> GetBookingSectionDetailsById(Guid bookingId)
        {
            var sql = @"
        SELECT 
            b.*, 
            r.rating, 
            r.review, 
            u.username
        FROM bookings b
        LEFT JOIN ratings r ON b.booking_id = r.booking_id
        LEFT JOIN users u ON b.student_id = u.UserId
        WHERE b.booking_id = @BookingId";

            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BookingSectionDetailsDTO>(sql, new { BookingId = bookingId });
        }
        public async Task<bool> UpdateBookingStatusAsync(Guid bookingId, string status)
        {
            var sql = "UPDATE bookings SET status = @Status WHERE booking_id = @BookingId";

            using var connection =_dapperContext.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Status = status, BookingId = bookingId });
            return result > 0;
        }
    }
}
