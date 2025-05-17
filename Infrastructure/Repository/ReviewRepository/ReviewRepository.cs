using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Application.DTO;
using Application.Interface.Repository;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository.ReviewRepository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DapperContext _Connection;
        public ReviewRepository(DapperContext connection)
        {
            _Connection = connection;
        }
        public async Task<int> AddReview(Review review)
        {
            using var connection = _Connection.CreateConnection();
          
            connection.Open();
            using var transaction =  connection.BeginTransaction();
            
            try
            {
                var checkSql = "SELECT COUNT(1) FROM ratings WHERE booking_id = @booking_id;";
                var exists = await connection.ExecuteScalarAsync<bool>(checkSql, new { review.booking_id }, transaction);
                if(exists)
                {
                var updaterating = @"UPDATE ratings SET rating = @rating, review = @review  WHERE booking_id = @booking_id;";
                
               
                    await connection.ExecuteAsync(updaterating, review, transaction);
                }
                else
                {

                    var insertSql = @"
        INSERT INTO ratings (rating_id, student_id, counselor_id, booking_id, rating, review)
        VALUES (UUID(), @student_id, @counselor_id, @booking_id, @rating, @review);";

                await connection.ExecuteAsync(insertSql, review, transaction);
                }


                var avgSql = "SELECT AVG(rating) FROM ratings WHERE counselor_id = @counselor_id;";
                var avgRating = await connection.ExecuteScalarAsync<decimal>(avgSql, new { review.counselor_id }, transaction);

               
                var updateSql = "UPDATE counselors SET avg_rating = @avgRating WHERE counselors_id = @counselor_id;";
                await connection.ExecuteAsync(updateSql, new { avgRating, review.counselor_id }, transaction);

             
                 transaction.Commit(); 
                return 1;
            }
            catch (Exception)
            {
                 transaction.Rollback(); 
                throw;
            }
        }



        public async Task<List<ReviewGetDTOStudent>> GetReviewsByCouncelorId(Guid Councelor_id)
        {

            var sql = @" SELECT *  FROM ratings r JOIN users u ON r.student_id = u.UserId
            WHERE r.counselor_id = @Councelor_id ORDER BY r.created_at DESC; ";
            using var connection = _Connection.CreateConnection();
            var result = await connection.QueryAsync<ReviewGetDTOStudent>(sql, new { Councelor_id });
            return result.ToList();


        }
        public async Task<AvrageRatingDTO?> GetReviewCountAndAverageRating(Guid counselorId)
        {
            var sql = @" SELECT avg_rating AS rating,(SELECT COUNT(*) FROM ratings WHERE counselor_id = @CounselorId) As reviews
              FROM counselors WHERE counselors_id = @CounselorId";

            using var connection = _Connection.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AvrageRatingDTO>(sql, new { CounselorId = counselorId });
        }
        public async Task<List<ReviewGetDTOStudent>> GetAllReviewsAsync()
        {
            var sql = @"SELECT r.*, u.username AS Username FROM ratings r JOIN users u ON r.student_id = u.UserId;";

            ;
            using var connection = _Connection.CreateConnection();
            var reviews = await connection.QueryAsync<ReviewGetDTOStudent>(sql);
            return reviews.ToList();
        }
        public async Task<bool> IsRatingExistsAsync(Guid bookingId)
        {
            var sql = "SELECT COUNT(*) FROM ratings WHERE booking_id = @bookingId;";
            using var connection = _Connection.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { bookingId });
            return count > 0;
        }
        public async Task<Review> GetReviewByBookingId(Guid bookingId)
        {
            var sql = "SELECT * FROM ratings WHERE booking_id = @bookingId;";
            using var connection = _Connection.CreateConnection();
            var count = await connection.QueryFirstOrDefaultAsync<Review>(sql, new { bookingId });
            return count;
        }
    }
}


