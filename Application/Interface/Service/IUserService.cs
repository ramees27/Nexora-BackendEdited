using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;

namespace Application.Interface.Service
{
    public interface IUserService
    {
        Task<ApiResponse<object>> RegisterUser(RegistrUserDTO dto);
        Task<ApiResponse<object>> Login(LoginDTO logindata);

        Task<ApiResponse<object>> RefreshAccessTocken(Guid userId);
        string GenerateRefreshToken();


    }
}
