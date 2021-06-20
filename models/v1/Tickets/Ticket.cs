using System;
namespace TicketingApi.Models.v1.Tickets
{
    public class Ticket
    {
        public int Id {get; set;}
        public string TicketNumber {get; set;}
        public string Subject {get;set;}
        public string Comment {get; set;}
        public int AppId {get; set;}
        public int ModuleId {get; set;}

        public string SenderMail {get; set;}
        public int StatId {get;set;}
        public string SolvedBy {get; set;}

        public DateTime? SolvedAt {get; set;}
        public string RejectedBy {get; set;}
        public string RejectedReason {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}

    }
}