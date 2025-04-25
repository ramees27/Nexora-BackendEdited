using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entities
{
    public class Notification
    {
      public Guid notification_id {  get; set; }
     public Guid sender_id {  get; set; }
     public Guid receiver_id {  get; set; }
      public string message {  get; set; }
     public bool is_read {  get; set; } 
     public DateTime created_at {  get; set; } 
    }
}
