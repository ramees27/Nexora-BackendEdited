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

namespace Infrastructure.Services.BookinService
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _Mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IMapper mapper, IBookingRepository bookingRepository, ILogger<BookingService> logger)
        {
            _Mapper = mapper;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        public async Task<ApiResponse<Guid>> BookingRequest(BookingRequestDto request, Guid userId, Guid Councelor_id)
        {
            try
            {
                var booking = _Mapper.Map<Booking>(request);
                booking.booking_id = Guid.NewGuid();
                booking.student_id = userId;
                booking.counselor_id = Councelor_id;

                var result = await _bookingRepository.AddBookingAsync(booking);

                if (result > 0)
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

                if (result)
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
        public async Task<ApiResponse<string>> UpdatePaymentAndStatusAsync(Guid bookingId)
        {
            try
            {
                var updated = await _bookingRepository.UpdateBookingPaymentAndStatusAsync(bookingId);
                if (!updated)
                    return new ApiResponse<string>
                    {
                        StatusCode = 404,
                        Message = "Booking not found or update failed",
                        Data = null
                    };

                return new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Payment status and booking status updated to accepted",
                    Data = null,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating payment and booking status for BookingId: {BookingId}", bookingId);
                return new ApiResponse<string>
                {
                    StatusCode = 599,
                    Message = "Internal server error",
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
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCompletedBookings(Guid studentId)
        {
            try
            {



                var bookings = await _bookingRepository.GetCompletedBookings(studentId);

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
        public async Task<ApiResponse<List<ActivityGetDTOForUser>>> GetCancelledorRejectedBookings(Guid studentId)
        {
            try
            {



                var bookings = await _bookingRepository.GetRejectedandCancelledBookings (studentId);

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
    }
}