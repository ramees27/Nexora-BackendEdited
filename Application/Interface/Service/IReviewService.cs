﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain;
using Domain.Entities;

namespace Application.Interface.Service
{
    public interface IReviewService
    {
        Task<ApiResponse<int>> AddReviews(ReviewAddDTO reviewAddDTO, Guid UserId, Guid councelor_id, Guid booking_id);
    }
}
