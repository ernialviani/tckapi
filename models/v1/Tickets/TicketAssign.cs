using System;
namespace TicketingApi.Models.v1.Tickets
{
    public class TicketAssign
    {
        public int Id {get;set;}
        public int TicketId {get; set;}
         public int? TeamId {get; set;}
         public DateTime? TeamAt {get; set;}
         public int? UserId {get; set;}
         public DateTime? UserAt {get; set;}
    }
}