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
using Infrastructure.Repository.BookinRepository;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;

namespace Infrastructure.Services.BookinService
{
    public class BookingServiceByCouncelor : IBookingServiceByCouncelor
    {
        private readonly IMapper _Mapper;
        private readonly IBookinRepositiryByCouncelor _bookingRepositoryByCouncelor;
        private readonly ILogger<BookingServiceByCouncelor> _logger;
        private readonly ICouncelorRepo _councelorRepo;
        public BookingServiceByCouncelor(IMapper mapper, IBookinRepositiryByCouncelor bookingRepository, ILogger<BookingServiceByCouncelor> logger, ICouncelorRepo councelorRep)
        {
            _Mapper = mapper;
            _bookingRepositoryByCouncelor = bookingRepository;
            _logger = logger;
            _councelorRepo = councelorRep;

        }
        public async Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetPendingRequestByCounselorId(Guid counselorId)
        {
            try
            {
                var isverify = await _councelorRepo.IsValidCounselor(counselorId);
                if (!isverify)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 404,
                        Message = "Councelor Not Found",
                        Data = null

                    };
                }
                var result = await _bookingRepositoryByCouncelor.GetPendingAndRequestedPaymentBookingsByCounselorId(counselorId);
                if (result.Count == 0)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 200,
                        Message = "No  Requests For Accepct",
                        Data = null

                    };
                }
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 200,
                    Message = "Requested Bookings succusfully Retrived",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Getting Requests");
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 500,
                    Message = "Error while Getting Requests",
                    Data = null

                };

            }




        }
        public async Task<ApiResponse<string>> RescheduleBookingAsync(BookingRescheduleDTO dto)
        {
            try
            {
                var updated = await _bookingRepositoryByCouncelor.RescheduleBookingAsync(dto);
                if (!updated)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = 404,
                        Message = "Booking not found or not updated"
                    };
                }

                return new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Booking rescheduled successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rescheduling booking");
                return new ApiResponse<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while rescheduling the booking"
                };
            }
        }
        public async Task<ApiResponse<List<BookingGetDTOByCouncelor>>> GetCancelledByCounselorId(Guid counselorId)
        {
            try
            {
                var isverify = await _councelorRepo.IsValidCounselor(counselorId);
                if (!isverify)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 404,
                        Message = "Councelor Not Found",
                        Data = null

                    };
                }
                var result = await _bookingRepositoryByCouncelor.GetCancelledBookingsByCounselorId (counselorId);
                if (result.Count == 0)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 200,
                        Message = "No  Requests Cancelled",
                        Data = null

                    };
                }
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 200,
                    Message = "Cancelled Bookings succusfully Retrived",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Getting Cancelled Bookings");
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 500,
                    Message = "Error while Getting Cancelled Bookings",
                    Data = null

                };

            }
        }
        public async Task <ApiResponse<List<BookingGetDTOByCouncelor>>> GetConfirmedBookings(Guid counselorId)
        {
            try
            {
                var isverify = await _councelorRepo.IsValidCounselor(counselorId);
                if (!isverify)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 404,
                        Message = "Councelor Not Found",
                        Data = null

                    };
                }
                var result = await _bookingRepositoryByCouncelor.GetUpcomingConfirmedBookings (counselorId);
                if (result.Count == 0)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 200,
                        Message = "No Bookings  Confirmed",
                        Data = null

                    };
                }
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 200,
                    Message = "Confirmed Bookings succusfully Retrived",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Getting Confirmed Bookings");
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 500,
                    Message = "Error while Getting Confirmed Bookings",
                    Data = null

                };

            }
        }
        public async Task<ApiResponse<List<BookingGetDTOByCouncelor>> >GetcompletedBookings(Guid counselorId)
        {
            try
            {
                var isverify = await _councelorRepo.IsValidCounselor(counselorId);
                if (!isverify)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 404,
                        Message = "Councelor Not Found",
                        Data = null

                    };
                }
                var result = await _bookingRepositoryByCouncelor.GetUpcompletedBookings (counselorId);
                if (result.Count == 0)
                {
                    return new ApiResponse<List<BookingGetDTOByCouncelor>>
                    {
                        StatusCode = 200,
                        Message = "No Bookings  Completed",
                        Data = null

                    };
                }
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 200,
                    Message = "Completed Bookings succusfully Retrived",
                    Data = result

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Getting Completed Bookings");
                return new ApiResponse<List<BookingGetDTOByCouncelor>>
                {
                    StatusCode = 500,
                    Message = "Error while Getting Completed Bookings",
                    Data = null

                };

            }
        }
        public async Task<ApiResponse<BookingSectionDetailsDTO>> GetBookingSectionDetails(Guid bookingId)
        {
            try
            {
                var result = await _bookingRepositoryByCouncelor.GetBookingSectionDetailsById(bookingId);

                if (result == null)
                {
                    return new ApiResponse<BookingSectionDetailsDTO>
                    {
                        StatusCode = 404,
                        Message = "Booking not found"
                    };
                }

                return new ApiResponse<BookingSectionDetailsDTO>
                {
                    StatusCode = 200,
                    Message = "Booking details fetched successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting booking section details");
                return new ApiResponse<BookingSectionDetailsDTO>
                {
                    StatusCode = 500,
                    Message = "Something went wrong"
                };
            }
        }

    }
}
