﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface ICouncelorRepo
    {
        Task<bool> ApplyAsCounselor(Counselor counselor, Guid userId);
        Task<CouncellorGetDTO> GetCounselorByIdAsync(Guid counselorId);
        Task<List<CouncellorGetDTO>> GetAllCouncelorAsync( );
        Task<List<CouncellorGetDTO>> GetCounselorsByKeyword(string keyword);
    }
}
