﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities;

namespace Application.Interface.Repository
{
    public interface IReviewRepository
    {
        Task<int> AddReview(Review review);
       
    }
}
