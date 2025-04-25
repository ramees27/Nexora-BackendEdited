using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Common.NHub;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace Infrastructure.Repository.NotificationRepository
{
    public  class NotificationRepository:INotificationRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRepository(DapperContext dapperContext,IHubContext<NotificationHub> hubContext)
        {
            _dapperContext = dapperContext;
            _hubContext = hubContext;
        }
        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            var sql = @"INSERT INTO notifications 
                    (notification_id, sender_id, receiver_id, message)
                    VALUES (@notification_id, @sender_id, @receiver_id, @message)";
           using  var connection=_dapperContext.CreateConnection();
           // await _hubContext.Clients.User(notification.receiver_id.ToString())
           //.SendAsync("ReceiveNotification", notification.message);


            return await connection.ExecuteAsync(sql, notification);
        }
        public async Task<BookingParticipantsDto> GetBookingParticipantsAsync(Guid bookingId)
        {
            var sql = @"SELECT student_id,counselor_id 
                FROM bookings 
                WHERE booking_id = @BookingId";

            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<BookingParticipantsDto>(sql, new { BookingId = bookingId });
        }

    }
}
