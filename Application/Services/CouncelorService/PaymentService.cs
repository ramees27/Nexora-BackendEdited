using System;
using System.Collections.Generic;
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
    public class PaymentService:IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICouncelorRepo _councelorRepo;
        private readonly ILogger<PaymentService> _logger;
      public PaymentService(IMapper mapper, IPaymentRepository paymentRepository, ICouncelorRepo councelorRepo,ILogger<PaymentService> logger )

        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _councelorRepo = councelorRepo;
            _logger = logger;
        }
        public async Task<ApiResponse<List<PaymentWithBookingDTO>>> GetPaymentWithBooking(Guid Councelorid)
        {
            try
            {
                var verify = await _councelorRepo.IsValidCounselor(Councelorid);
                if (!verify)
                {
                    return new ApiResponse<List<PaymentWithBookingDTO>>
                    {
                        StatusCode = 404,
                        Message = "Councelor Not found",
                        Data = null
                    };
                }
                var result = await _paymentRepository.GetAllPaymentsWithBookingAsync(Councelorid);
                if (!result.Any()|| result==null)
                {
                    return new ApiResponse<List<PaymentWithBookingDTO>>
                    {
                        StatusCode = 200,
                        Message = "No Payments",
                        Data = result
                    };

                }
                return new ApiResponse<List<PaymentWithBookingDTO>>
                {
                    StatusCode = 200,
                    Message = "Successfully retrived",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error While fetching details");
                return new ApiResponse<List<PaymentWithBookingDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching counselor details",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<PaymentSummaryDTO>> GetPaymentDetails(Guid counselorId)
        {
            try
            {
                var verify = await _councelorRepo.IsValidCounselor(counselorId);
                if (!verify)
                {
                    return new ApiResponse<PaymentSummaryDTO>
                    {
                        StatusCode = 404,
                        Message = "No Councelor Found",
                        Data = null,


                    };
                }
                var result = await _paymentRepository.GetEarningsBreakdownAsync(counselorId);
                
              
                return new ApiResponse<PaymentSummaryDTO>
                {
                    StatusCode = 200,
                    Message = "Earnings Breakdown Retrieved Successfully",
                    Data = result,
                };

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occured while calculating total earnings");
                return new ApiResponse<PaymentSummaryDTO>
                {
                    StatusCode = 500,
                    Message = "An Error Occured"
                };
            }
           
        }
        
    }
}
