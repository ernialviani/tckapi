using System;
namespace TicketingApi.Models.v1.Tickets
{
    public class TicketDetail
    {
        public int Id {get;set;}
        public int TicketId {get; set;}
         public string UserMail {get; set;}
         public string Comment {get; set;}
         public string Flag {get; set;}
         public DateTime? CreatedAt {get; set;}
    
         public DateTime? UpdatedAt {get; set;}
    }
}