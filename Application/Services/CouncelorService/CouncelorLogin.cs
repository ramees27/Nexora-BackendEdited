﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interface.Repository;
using Application.Interface.Service;
using AutoMapper;
using Domain;
using Domain.Entities;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.CouncelorService
{
    public class CouncelorLogin : ICouncelorService
    {
        private readonly ILogger<CouncelorLogin> _logger;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ICouncelorRepo _councelorRepo;
        public CouncelorLogin(ILogger<CouncelorLogin> logger, ICloudinaryService cloudinaryService, IMapper mapper, ICouncelorRepo councelorRepo)
        {
            _logger = logger;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _councelorRepo = councelorRepo;

        }
        public async Task<ApiResponse<object>> ApplyAsCounselor(CounselorAddDTO dto, Guid userId)
        {
            try
            {
                var existing = await _councelorRepo.IsActiveCounselor(userId); // You should create this method if needed
                if (existing)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "You have already applied as a counselor"
                    };
                }

                string? imageUrl = null;
                if (dto.ProfileImage != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(dto.ProfileImage);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        return new ApiResponse<object>
                        {
                            StatusCode = 400,
                            Message = "Image upload failed"
                        };
                    }
                }
                var councelor = _mapper.Map<Counselor>(dto);

                councelor.image_url = imageUrl;
                councelor.counselors_id = Guid.NewGuid();
                Console.WriteLine("/n /n /n " + councelor.counselors_id + "/n /n /n ");


                var isSaved = await _councelorRepo.ApplyAsCounselor(councelor, userId);
                if (!isSaved)
                {
                    return new ApiResponse<object>
                    {
                        StatusCode = 500,
                        Message = "Failed to apply as counselor"
                    };
                }
                return new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Counselor application submitted successfully",
                    Data = councelor.counselors_id
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while applying as counselor");
                return new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred",
                    Error = ex.Message
                };
            }
        }
        public async Task<ApiResponse<CouncellorGetDTO>> GetCounselorById(Guid counselorId)
        {
            _logger.LogInformation("Start: Getting counselor by ID - {CounselorId}", counselorId);
            try
            {
                var counselor = await _councelorRepo.GetCounselorByIdAsync(counselorId);

                if (counselor == null)
                {
                    _logger.LogWarning("Counselor not found for ID - {CounselorId}", counselorId);

                    return new ApiResponse<CouncellorGetDTO>
                    {
                        StatusCode = 404,
                        Message = "Counselor not found",
                        Data = null,
                        Error = "Invalid ID or counselor might be deleted"
                    };
                }
                _logger.LogInformation("Success: Counselor retrieved for ID - {CounselorId}", counselorId);

                return new ApiResponse<CouncellorGetDTO>
                {
                    StatusCode = 200,
                    Message = "Counselor retrieved successfully",
                    Data = counselor,
                    Error = null
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while retrieving counselor by ID - {CounselorId}", counselorId);

                return new ApiResponse<CouncellorGetDTO>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching counselor details",
                    Data = null,
                    Error = ex.Message
                };
            }

        }
        public async Task<ApiResponse<List<CouncellorGetDTO>>> GetAllCounselorsAsync()
        {
            _logger.LogInformation("Start: Getting all counselors");

            try
            {
                var result = await _councelorRepo.GetAllCouncelorAsync();


                _logger.LogInformation("Success: Retrieved {Count} counselors", result.Count);

                return new ApiResponse<List<CouncellorGetDTO>>
                {
                    StatusCode = 200,
                    Message = "Counselors fetched successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all counselors");

                return new ApiResponse<List<CouncellorGetDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred",
                    Data = null,
                    Error = ex.Message
                };
            }

        }
        public async Task<ApiResponse<List<CouncellorGetDTO>>> GetCounselorsByKeywords(string keyword)
        {
            _logger.LogInformation(" SearchCounselorsByKeyword started with keyword: {Keyword}", keyword);

            try
            {
                var counselors = await _councelorRepo.GetCounselorsByKeyword(keyword);


                if (counselors == null || !counselors.Any())
                {
                    return new ApiResponse<List<CouncellorGetDTO>>
                    {
                        StatusCode = 404,
                        Message = "No counselors found",
                        Data = null
                    };
                }
                var sorted = counselors.OrderByDescending(c => c.avg_rating).ToList();

                _logger.LogInformation("Found {Count} counselors for keyword: {Keyword}", sorted.Count, keyword);

                return new ApiResponse<List<CouncellorGetDTO>>
                {
                    StatusCode = 200,
                    Message = "Counselors retrieved successfully",
                    Data = sorted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred in SearchCounselorsByKeyword.");
                return new ApiResponse<List<CouncellorGetDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving counselors",
                    Error = ex.Message
                };

            }

        }
        public async Task<ApiResponse<object>> AddEducationAsync(EducationCreateDTO dto)
        {
            try
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(dto.CertificateImage);

                var education = new Education
                {
                    education_id = Guid.NewGuid(),
                    counselor_id = dto.CounselorId,
                    qualification = dto.Qualification,
                    certificate_image_url = imageUrl
                };

                var success = await _councelorRepo.AddEducationAsync(education);

                if (!success)
                    return new ApiResponse<object> { StatusCode = 500, Message = "Failed to add education", Data = null };

                return new ApiResponse<object> { StatusCode = 200, Message = "Education added successfully", Data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding education");
                return new ApiResponse<object> { StatusCode = 500, Message = "An error occurred", Data = null, Error = ex.Message };
            }
        }
        public async Task<ApiResponse<List<EducationDTO>>> GetEducationByCounselorIdAsync(Guid counselorId)
        {
            try
            {
                var response = await _councelorRepo.GetEducationByCounselorIdAsync(counselorId);
                return new ApiResponse<List<EducationDTO>>
                {
                    StatusCode = 200,
                    Message = "Education fetched successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding education");
                return new ApiResponse<List<EducationDTO>> { StatusCode = 500, Message = "An error occurred", Data = null, Error = ex.Message };
            }
        }
        public async Task<CounselorStatusDTO> GetCounselorStatusAsync(Guid userId)
        {
            return await _councelorRepo.CheckCounselorStatusAsync(userId);
        }
        public async Task<ApiResponse<object?>> getCounseloridByUserId(Guid userId)
        {
            try
            {
                var result = await _councelorRepo.getCounseloridByUserId(userId);

                if (result == null)
                {
                    return new ApiResponse<object?>
                    {
                        StatusCode = 404,
                        Message = "Counselor not found for the given user ID.",
                        Data = null
                    };
                }

                return new ApiResponse<object?>
                {
                    StatusCode = 200,
                    Message = "Retrieved counselor ID",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object?>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving the counselor ID.",
                    Data = null,
                    Error = ex.Message
                };
            }
        }


    }
}
