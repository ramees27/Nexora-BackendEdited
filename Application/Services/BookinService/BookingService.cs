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

namespace Infrastructure.Services.BookinService
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _Mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;
        private readonly INotificationRepository _notificationRepository;
        public BookingService(IMapper mapper, IBookingRepository bookingRepository, ILogger<BookingService> logger, INotificationRepository notificationRepository)
        {
            _Mapper = mapper;
            _bookingRepository = bookingRepository;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }
        public async Task<ApiResponse<Guid>> BookingRequest(BookingRequestDto request, Guid userId, Guid Councelor_id)
        {
            try
            {
                //var booking = _Mapper.Map<Booking>(request);
                //booking.booking_id = Guid.NewGuid();
                //booking.student_id = userId;
                //booking.counselor_id = Councelor_id;
                var booking = new Booking
                {
                    booking_id = Guid.NewGuid(),
                    preferd_date = request.PreferdDate,
                    preferd_time = request.PreferdTime,
                    student_id = userId,
                    counselor_id = Councelor_id,


                };

                var result = await _bookingRepository.AddBookingAsync(booking, Councelor_id);
                var notification = new Notification
                {
                    notification_id = Guid.NewGuid(),
                    sender_id = userId,
                    receiver_id = Councelor_id,
                    message = "A new booking request is received, Request payment or reshedule the date and time"
                };
                var notificationresult = await _notificationRepository.CreateNotificationAsync(notification);

                if (result > 0 && notificationresult > 0)
                {
                    _logger.LogInformation("Booking created: {BookingId}", booking.booking_id);
                    return new ApiResponse<Guid>
                    {
                        StatusCode = 200,
                        Message = "Booking created successfully",
                        Data = booking.booking_id,
                    };
                }

                return new ApiResponse<Guid>
                {
                    StatusCode = 400,
                    Message = "Booking creation failed",
                    Data = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating booking");
                return new ApiResponse<Guid>
                {
                    StatusCode = 500,
                    Message = "Internal server error",
                    Data = Guid.Empty,
                    Error = ex.Message
                };
            }
        }
        public async Task<ApiResponse<string>> UpdateStatusAsync(Guid bookingId, string status)
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

                var result = await _bookingRepository.UpdateBookingStatusAsync(bookingId, status.ToLower());
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
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetPendingRequestPaymentBookings(Guid studentId)
        {
            try
            {
                _logger.LogInformation("Fetching pending or request payment bookings for student {StudentId}", studentId);


                var bookings = await _bookingRepository.GetPendingRequestPayemntBookings(studentId);

                if (bookings == null || !bookings.Any())
                {
                    _logger.LogWarning("No bookings found for student {StudentId} with pending or request payment status", studentId);
                    return new ApiResponse<List<ActivityGetDTOForUser>>
                    {
                        StatusCode = 200,
                        Message = "No bookings found",
                        Data = null
                    };
                }

                _logger.LogInformation("Found bookings for students");

                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 200,
                    Message = "Bookings retrieved successfully",
                    Data = bookings.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bookings for student {StudentId}", studentId);
                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request",
                    Data = null
                };
            }


        }
        public async Task<ApiResponse<string>> UpdatePaymentAndStatusAsync(Guid bookingId)
        {
            try
            {
                var updated = await _bookingRepository.UpdateBookingPaymentAndStatusAsync(bookingId);
                var getId = await _notificationRepository.GetBookingParticipantsAsync(bookingId);


                var notification = new Notification
                {
                    notification_id = Guid.NewGuid(),
                    receiver_id = getId.counselor_id,
                    sender_id = getId.student_id,
                    message = $"Payment Completed By student Booking id={bookingId}"
                };
                var notificationresult = await _notificationRepository.CreateNotificationAsync(notification);

                if (!updated || notificationresult <= 0)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = 404,
                        Message = "Booking not found or update failed",
                        Data = null
                    };
                }

                return new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Booking status updated and payment recorded successfully.",
                    Data = "Success",
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing booking payment and status update.", bookingId);
                return new ApiResponse<string>
                {
                    StatusCode = 599,
                    Message = "An unexpected error occurred.",
                    Data = null,
                };
            }
        }
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetBookingsByScheduled(Guid studentId)
        {
            try
            {



                var bookings = await _bookingRepository.GetScheduledBookings(studentId);

                if (bookings == null || !bookings.Any())
                {

                    return new ApiResponse<List<ActivityGetDTOForUser>>
                    {
                        StatusCode = 200,
                        Message = "No bookings found",
                        Data = null
                    };
                }

                _logger.LogInformation("Found bookings for students");

                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 200,
                    Message = "Bookings retrieved successfully",
                    Data = bookings.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bookings for student {StudentId}", studentId);
                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request",
                    Data = null
                };
            }


        }
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCompletedBookings(Guid studentId)
        {
            try
            {



                var bookings = await _bookingRepository.GetCompletedBookings(studentId);

                if (bookings == null || !bookings.Any())
                {

                    return new ApiResponse<List<ActivityGetDTOForUser>>
                    {
                        StatusCode = 200,
                        Message = "No bookings found",
                        Data = null
                    };
                }

                _logger.LogInformation("Found bookings for students");

                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 200,
                    Message = "Bookings retrieved successfully",
                    Data = bookings.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bookings for student {StudentId}", studentId);
                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request",
                    Data = null
                };
            }


        }
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCancelledorRejectedBookings(Guid studentId)
        {
            try
            {



                var bookings = await _bookingRepository.GetRejectedandCancelledBookings(studentId);

                if (bookings == null || !bookings.Any())
                {

                    return new ApiResponse<List<ActivityGetDTOForUser>>
                    {
                        StatusCode = 404,
                        Message = "No bookings found",
                        Data = null
                    };
                }

                _logger.LogInformation("Found bookings for students");

                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 200,
                    Message = "Bookings retrieved successfully",
                    Data = bookings.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bookings for student {StudentId}", studentId);
                return new ApiResponse<List<ActivityGetDTOForUser>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request",
                    Data = null
                };
            }

        }
        public async Task <ApiResponse<bool>>CancelBookingByUserAfterPayment(Guid bookingId)
        {
            try
            {
                var updated = await _bookingRepository.CancelBookingByUserAfterPayment(bookingId);
                //var getId = await _notificationRepository.GetBookingParticipantsAsync(bookingId);


                //var notification = new Notification
                //{
                //    notification_id = Guid.NewGuid(),
                //    receiver_id = getId.counselor_id,
                //    sender_id = getId.student_id,
                //    message = $"Payment Completed By student Booking id={bookingId}"
                //};
                //var notificationresult = await _notificationRepository.CreateNotificationAsync(notification);

               

                return new ApiResponse<bool>
                {
                    StatusCode = 200,
                    Message = "Booking Cancelled succusfully.",
                    Data = updated,
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing booking payment and status update.", bookingId);
                return new ApiResponse<bool>
                {
                    StatusCode = 599,
                    Message = "An unexpected error occurred.",
                   
                };
            }
        }
    }
}