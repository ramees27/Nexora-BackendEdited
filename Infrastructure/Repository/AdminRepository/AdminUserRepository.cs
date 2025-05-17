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
using MySqlX.XDevAPI.Common;

namespace Infrastructure.Repository.AdminRepository
{
    public class AdminUserRepository:IAdminUserRepository
    { 
        private readonly DapperContext _dappercontext;
        public AdminUserRepository(DapperContext dappercontext)
        {
            _dappercontext = dappercontext;
        }
        public async Task<List<UserDetailsDTO>> GetAllStudentsForUserManagementAsync()
        {
            var sql = @"SELECT *  FROM users u WHERE u.UserId NOT IN (SELECT c.user_id  FROM counselors c WHERE c.is_verified = TRUE) AND u.role != 'admin'
"

            ;

            using var connection = _dappercontext.CreateConnection();
            var result = await connection.QueryAsync<UserDetailsDTO>(sql);
            return result.ToList();

        }
        public async Task<List<CounselorDetailsDTO>> GetverifiedCounselorsAsync()
        {
            var sql = @"SELECT  c.*, u.userEmail AS email, e.education_id,  e.qualification,  e.certificate_image_url,  e.counselor_id
FROM counselors c
inner JOIN users u ON c.user_id = u.UserId
left JOIN education e ON c.counselors_id = e.counselor_id
"
 ;




            using var connection = _dappercontext.CreateConnection();
                var result = await connection.QueryAsync<CounselorDetailsDTO>(sql);
                return result.ToList();
            
        }
        public async Task<bool> SoftDeleteCounselorAsync(Guid id)
        {
            var query = "UPDATE counselors SET is_deleted = 1 WHERE counselors_id = @Id";
            using var connection = _dappercontext .CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return result > 0;

        }
        public async Task<bool> SoftDeleteStudentAsync(Guid id)
        {
            var query = "UPDATE users SET is_deleted = 1 WHERE UserId = @Id";
            using var connection = _dappercontext.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return result > 0;

        }
    }
}
