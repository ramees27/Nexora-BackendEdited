using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Application.Interface.Repository;
using Application.DTO;
using Dapper;
using Infrastructure.Data;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Ocsp;
using Domain.Entities;

namespace Infrastructure.Repository.AuthRepository
{
    public class UserRepository : IUserRepository

    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task RegisterUser(RegistrUserDTO registrationDTO)
        {
            var query = @"Insert into users(UserId , username,Useremail,passwordHash,created_at)VALUES (UUID(), @username, @Email, @PasswordHash, NOW());";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Username = registrationDTO.userName, Email = registrationDTO.email, PasswordHash = registrationDTO.password });
            }

        }
        public async Task<User> GetUserName(string userName)
        {
            var sql = "Select * from users where username=@userName";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = userName });
        }
        public async Task<User> GetUsermail(string email)
        {
            //string sql = "SELECT * FROM users WHERE Useremail = @Useremail";
            var sql = "SELECT * FROM users WHERE UserEmail = @UserEmail";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserEmail = email });
            ;
        }

        public async Task<User> CheckRefreshToken(Guid UserId)
        {
            var sql = "select * from users where UserId =@UserID";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = UserId });
        }

        public async Task UpdateRefreshToken(Guid userid, string newrefreshtoken)
        {
            var sql = "UPDATE users SET refresh_token = @NewRefreshToken, token_expiry = @TokenExpiry WHERE UserId = @UserID";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                UserID = userid,
                NewRefreshToken = newrefreshtoken, // ✅ Match SQL variable name
                TokenExpiry = DateTime.Now.AddDays(7)
            });
        }

    }
}
