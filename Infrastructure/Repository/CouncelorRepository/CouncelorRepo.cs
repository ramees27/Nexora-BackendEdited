using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Infrastructure.Data;
using Infrastructure.Services.CloudinaryService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Repository.CouncelorRepository
{
    public class Councelor : ICouncelorRepo
    {
        private readonly DapperContext _context;
     
        public Councelor(DapperContext context)
        {
            _context = context;
        }
        public async Task<bool> ApplyAsCounselor(Counselor counselor, Guid userId)
        {
          

            counselor.user_id = userId;
            var sql = @"INSERT INTO counselors 
         ( counselors_id, user_id, full_name, specialization, short_bio, mobile_number, is_verified, experience,  hourly_rate, upi_id, avg_rating, image_url, is_deleted)
          VALUES 
          (@counselors_id, @user_id, @full_name, @specialization, @short_bio, @mobile_number, @is_verified, @experience, @hourly_rate, @upi_id, @avg_rating, @image_url, @is_deleted)";
           
            using  var connection=_context.CreateConnection();
            var result =await connection.ExecuteAsync(sql, counselor);
            return result > 0;
        
        
        }
        public async Task<CouncellorGetDTO> GetCounselorByIdAsync(Guid counselorId)
        {
            var sql = @"SELECT c.*,  u.UserName,u.UserEmail FROM counselors c JOIN users u ON c.user_id = u.UserId WHERE counselors_id = @CounselorsId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CouncellorGetDTO>(sql, new { CounselorsId = counselorId });
        }
        public async Task<List<CouncellorGetDTO>> GetAllCouncelorAsync() 
        { 
        var sql = @"SELECT c.*,  u.UserName,u.UserEmail FROM counselors c JOIN users u ON c.user_id = u.UserId  WHERE c.is_deleted = FALSE AND c.is_verified = TRUE";
            using var connection = _context.CreateConnection();
             var councelors=await connection.QueryAsync<CouncellorGetDTO>(sql);
            return councelors.ToList();
        }
        public async Task<List<CouncellorGetDTO>> GetCounselorsByKeyword(string keyword)
        {
            var sql = @"SELECT c.*, u.UserName, u.UserEmail 
FROM counselors c 
JOIN users u ON c.user_id = u.UserId  
WHERE c.is_deleted = FALSE 
  AND c.is_verified = TRUE  
  AND (c.specialization LIKE @Keyword ) 
ORDER BY c.avg_rating DESC
";
            using var connection = _context.CreateConnection();
            var result=await connection.QueryAsync<CouncellorGetDTO>(sql, new { Keyword = $"%{keyword}%" });
            return result.ToList();
        }
        public async Task<bool> IsValidCounselor(Guid counselorId)
        {
            var sql = @"
        SELECT COUNT(1)
        FROM counselors
        WHERE counselors_id = @CounselorId AND is_verified = TRUE AND is_deleted = FALSE"
            ;

            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { CounselorId = counselorId });

            return count > 0;
        }
        public async Task<bool> IsActiveCounselor(Guid UserId)
        {
            var sql = @"
        SELECT COUNT(1)
        FROM counselors
        WHERE user_id = @user_id AND is_deleted = FALSE";

            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { user_id = UserId });

            return count > 0;
        }
        public async Task<bool> AddEducationAsync(Education education)
        {
            var sql = @"INSERT INTO education (education_id, counselor_id, qualification, certificate_image_url)
                    VALUES (@education_id, @counselor_id, @qualification, @certificate_image_url)";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, education);
            return result > 0;
        }
        //public async Task<bool> AddRatingAsync(Education education)
        //{

        //    var sql = @"INSERT INTO ratings (rating_id, counselor_id, user_id, rating_value, comment)
        //        VALUES (@rating_id, @counselor_id, @user_id, @rating_value, @comment)";

        //    using var connection = _context.CreateConnection();
        //    var result = await connection.ExecuteAsync(sql, rating);
        //    return result > 0;
        //}
        public async Task<List<EducationDTO>> GetEducationByCounselorIdAsync(Guid counselorId)
        {
            var sql = "SELECT * FROM education WHERE counselor_id = @CounselorId";
            using var connection = _context.CreateConnection();
            var result= await connection. QueryAsync<EducationDTO>(sql, new { CounselorId = counselorId });
            return result.ToList();
        }
        public async Task<CounselorStatusDTO> CheckCounselorStatusAsync(Guid userId)
        {
            var query = @"SELECT is_verified, is_deleted FROM counselors WHERE user_id = @UserId LIMIT 1";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, new { UserId = userId });

            if (result == null)
            {
                return new CounselorStatusDTO
                {
                    Exists = false,
                    IsVerified = false,
                    IsDeleted = false
                };
            }

            return new CounselorStatusDTO
            {
                Exists = true,
                IsVerified = result.is_verified,
                IsDeleted = result.is_deleted
            };
        }
        public async Task<Guid?> getCounseloridByUserId(Guid userId)
        {
            var query = @"SELECT counselors_id FROM counselors WHERE user_id = @UserId";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Guid?>(query, new { UserId = userId });

            return result;
        }
    }
}
