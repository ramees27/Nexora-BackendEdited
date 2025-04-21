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

namespace Infrastructure.Repository.ReviewRepository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly DapperContext _Connection;
        public ReviewRepository(DapperContext connection)
        {
            _Connection = connection;
        }
        public async Task<int> AddReview(Review review)
        {

            var sql = @"
            INSERT INTO ratings (rating_id,student_id, counselor_id, booking_id, rating, review)
            VALUES (rating_id,@student_id, @counselor_id, @booking_id, @rating, @review);";
            using var connection=_Connection.CreateConnection();
            return await connection.ExecuteAsync(sql, review);


        }
        public async Task<List<ReviewGetDTOStudent>> GetReviewsByCouncelorId(Guid Councelor_id)
        {
            
                var sql = @" SELECT *  FROM ratings r JOIN users u ON r.student_id = u.UserId
            WHERE r.counselor_id = @Councelor_id ORDER BY r.created_at DESC; ";
                using var connection=_Connection.CreateConnection();
                var result= await connection.QueryAsync<ReviewGetDTOStudent>(sql, new { Councelor_id });
                return result.ToList();

            
        }
        public async Task<AvrageRatingDTO> GetReviewCountAndAverageRating(Guid counselorId)
        {
            var sql = @"
                SELECT 
                    COUNT(*) AS reviews,
                    AVG(rating * 1.0) AS rating
                FROM ratings
                WHERE counselor_id = @CounselorId";
            using var connection=_Connection.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AvrageRatingDTO>(sql, new { CounselorId = counselorId });
        }
    }
}
 

