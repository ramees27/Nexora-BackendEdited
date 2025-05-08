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
using Infrastructure.Repository;

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
        private readonly INotificationRepository _notificationRepository;
        public BookingServiceByCouncelor(IMapper mapper, IBookinRepositiryByCouncelor bookingRepository, ILogger<BookingServiceByCouncelor> logger, ICouncelorRepo councelorRep,
                                              INotificationRepository notificationRepository )
        {
            _Mapper = mapper;
            _bookingRepositoryByCouncelor = bookingRepository;
            _logger = logger;
            _councelorRepo = councelorRep;
            _notificationRepository = notificationRepository;
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
        public async Task<ApiResponse<string>> UpdateStatusByCouncelor(Guid bookingId, string status)
        {
            try
            {
                var validStatuses = new[] { "pending", "request_payment", "accepted", "declined", "cancelled", "completed" };

                if (!validStatuses.Contains(status.ToLower()))
                {
                    _logger.LogWarning("Invalid status update attempted: {Status}", status);
                    return new ApiResponse<string>
                    {
                        StatusCode = 400,
                        Message = "Invalid status",
                        Error = "Unsupported status value"
                    };
                }

                var result = await _bookingRepositoryByCouncelor.UpdateBookingStatusAsync(bookingId, status.ToLower());
                var getId = await _notificationRepository.GetBookingParticipantsAsync(bookingId);
                var notification = new Notification
                {
                    notification_id = Guid.NewGuid(),
                    receiver_id = getId.counselor_id,
                    sender_id = getId.student_id,
                    message = $"Booking Status Updated bookingId:-{bookingId}"
                };
                var notificationresult = await _notificationRepository.CreateNotificationAsync(notification);



                if (result && notificationresult > 0)
                {
                    _logger.LogInformation("Booking status updated to {Status} for BookingId: {BookingId}", status, bookingId);
                    return new ApiResponse<string>
                    {
                        StatusCode = 200,
                        Message = "Status updated successfully",
                        Data = status
                    };
                }


                _logger.LogWarning("Booking status update failed or not found. BookingId: {BookingId}", bookingId);
                return new ApiResponse<string>
                {
                    StatusCode = 404,
                    Message = "Booking not found or status unchanged",
                    Error = "No update occurred"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating booking status for BookingId: {BookingId}", bookingId);
                return new ApiResponse<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while updating status",
                    Error = ex.Message
                };
            }
        }
    }
}
