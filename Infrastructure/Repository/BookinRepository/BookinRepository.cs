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
using Infrastructure.Repository.CouncelorRepository;

namespace Infrastructure.Repository.BookinRepository
{
    public class BookinRepository : IBookingRepository
    {
        private readonly DapperContext _context;

        public BookinRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddBookingAsync(Booking booking)
        {
            var sql = @"INSERT INTO bookings 
               (booking_id, student_id, counselor_id, preferd_time, preferd_date) 
               VALUES 
               (@Id, @StudentId, @CounselorId, @PreferdTime, @PreferdDate);";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(sql, booking);


        }
        public async Task<bool> UpdateBookingStatusAsync(Guid bookingId, string status)
        {
            var sql = "UPDATE bookings SET status = @Status WHERE booking_id = @BookingId";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { Status = status, BookingId = bookingId });
            return result > 0;
        }
        public async Task<List<ActivityGetDTOForUser>> GetPendingRequestPayemntBookings(Guid studentId)
        {
            var sql = @"SELECT * FROM bookings b JOIN counselors c ON b.counselor_id = c.counselors_id JOIN users u ON c.user_id = u.userId
        WHERE b.student_id = @StudentId   AND b.status IN ('pending', 'request_payment')  ORDER BY b.created_at DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ActivityGetDTOForUser>(sql, new { StudentId = studentId });
            return result.ToList();
        }
        public async Task<bool> UpdateBookingPaymentAndStatusAsync(Guid bookingId)
        {
            var sql = @"UPDATE bookings 
                SET payment_status = TRUE, status = 'accepted' 
                WHERE booking_id = @BookingId";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { BookingId = bookingId });
            return result > 0;
        }
        public async Task<List<ActivityGetDTOForUser>> GetScheduledBookings(Guid studentId)
        {
            var sql = @"SELECT * FROM bookings b JOIN counselors c ON b.counselor_id = c.counselors_id JOIN users u ON c.user_id = u.userId
         WHERE b.student_id = @StudentId 
        AND b.status = 'accepted' AND b.payment_status = true  ORDER BY b.created_at DESC";
        

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ActivityGetDTOForUser>(sql, new { StudentId = studentId });
            return result.ToList();
        }
        public async Task<List<ActivityGetDTOForUser>> GetCompletedBookings(Guid studentId)
        {
            var sql = @"SELECT * FROM bookings b JOIN counselors c ON b.counselor_id = c.counselors_id JOIN users u ON c.user_id = u.userId
         WHERE b.student_id = @StudentId 
        AND b.status = 'completed' AND b.payment_status = true  ORDER BY b.created_at DESC";


            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ActivityGetDTOForUser>(sql, new { StudentId = studentId });
            return result.ToList();
        }
        public async Task<List<ActivityGetDTOForUser>> GetRejectedandCancelledBookings(Guid studentId)
        {
            var sql = @"SELECT * FROM bookings b JOIN counselors c ON b.counselor_id = c.counselors_id JOIN users u ON c.user_id = u.userId
        WHERE b.student_id = @StudentId   AND b.status IN ('declined', 'cancelled')  ORDER BY b.created_at DESC";


            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ActivityGetDTOForUser>(sql, new { StudentId = studentId });
            return result.ToList();

        }
    }
}
