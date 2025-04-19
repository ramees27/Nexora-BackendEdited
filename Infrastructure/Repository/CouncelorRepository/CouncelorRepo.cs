using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using AutoMapper;
using Dapper;
using Domain.Entities;
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
          (UUID(), @user_id, @full_name, @specialization, @short_bio, @mobile_number, @is_verified, @experience, @hourly_rate, @upi_id, @avg_rating, @image_url, @is_deleted)";
           
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
            var sql = @"SELECT c.*,  u.UserName,u.UserEmail FROM counselors c JOIN users u ON c.user_id = u.UserId  WHERE c.is_deleted = FALSE AND c.is_verified = TRUE  
               AND (c.specialization LIKE @Keyword OR c.short_bio LIKE @Keyword) ORDER BY c.avg_rating DESC";
            using var connection = _context.CreateConnection();
            var result=await connection.QueryAsync<CouncellorGetDTO>(sql, new { Keyword = $"%{keyword}%" });
            return result.ToList();
        }
    }
}
