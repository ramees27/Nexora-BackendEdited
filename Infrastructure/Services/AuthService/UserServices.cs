using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Application.Interface.Service;
using Domain;
using Common.Helpers;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.AuthService
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserServices> _logger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserServices(IUserRepository userRepository, ILogger<UserServices> logger, IJwtTokenGenerator jwtTokenGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<object>> RegisterUser(RegistrUserDTO dto)
        {
            try

            {
                if (string.IsNullOrWhiteSpace(dto.userName) ||
                 string.IsNullOrWhiteSpace(dto.email) ||
                 string.IsNullOrWhiteSpace(dto.password))
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "Validation failed",
                        Error = "Username, Email, and Password are required."
                    };

                }
                var existingUser = await _userRepository.GetUserName(dto.userName);

                if (existingUser != null)
                {
                    return new ApiResponse<object> { StatusCode = 409, Message = "User Name Already exist " };
                }
                var existingmail = await _userRepository.GetUsermail(dto.email);

                if (existingmail != null)
                {
                    return new ApiResponse<object> { StatusCode = 409, Message = "User  Already exist " };
                }


                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.password);


                await _userRepository.RegisterUser(new RegistrUserDTO
                {
                    userName = dto.userName,
                    email = dto.email,
                    password = hashedPassword
                });
                return new ApiResponse<object> { StatusCode = 200, Message = "Registration Successful" };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user");

                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "Registration failed",
                    Error = ex.Message
                };
            }
        }
        public async Task<ApiResponse<object>> Login(LoginDTO logindata)
        {
            try
            {
                var user = await _userRepository.GetUsermail(logindata.Email);
                if (user == null)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 401,
                        Message = "Invalid email"
                    };
                }
                if (user.Is_Deleted)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "You are blocked by admin"
                    };
                }
                var verifyPassword = BCrypt.Net.BCrypt.Verify(logindata.Password, user.PasswordHash);
                if (!verifyPassword)
                {
                    return new ApiResponse<object> { StatusCode = 401, Message = "Inalid Password" };

                }
                var token = _jwtTokenGenerator.GenerateToken(user);
                var context = _httpContextAccessor.HttpContext;

                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                var refreshTocken = GenerateRefreshToken();
                await _userRepository.UpdateRefreshToken(user.UserId, refreshTocken);
                return new ApiResponse<object> { StatusCode = 200, Message = "Login success", Data = new { Token = token, user.Role, } };



            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Error occurred while Login user");

                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "Login failed",
                    Error = ex.Message
                };

            }
        }
        public async Task<ApiResponse<object>> RefreshAccessTocken(Guid userId)
        {
            try
            {
                var user = await _userRepository.CheckRefreshToken(userId);
                if (user == null || user.Token_Expiry < DateTime.Now)
                {
                    return new ApiResponse<object> { StatusCode = 401, Message = "Invalid or expired refresh token" };
                }
                var context = _httpContextAccessor.HttpContext;

                var accessToken = _jwtTokenGenerator.GenerateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                await _userRepository.UpdateRefreshToken(user.UserId, newRefreshToken);
                return new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Token Refreshed Successfully",

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Secceion Expired");

                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "Tocken generation failed",
                    Error = ex.Message
                };
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
    }

}
