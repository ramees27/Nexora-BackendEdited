using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface IUserRepository
    {
         Task RegisterUser(RegistrUserDTO registrationDTO);
        Task <User> GetUserName(string userName);
        Task<User> GetUsermail(string email);
        Task <User> CheckRefreshToken(Guid UserId);
        Task UpdateRefreshToken(Guid userid, string newrefreshtoken);
       

    }
}
