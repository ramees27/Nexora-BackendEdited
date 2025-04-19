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
    }
}

