using System.Collections;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int SenderId {get; set;}
        public int StatId {get;set;}
        public string SolvedBy {get; set;}
        public DateTime? SolvedAt {get; set;}
        public string RejectedBy {get; set;}
        public string RejectedReason {get;set;}
        public DateTime? RejectedAt {get; set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}


        public ICollection<TicketDetail> TicketDetails {get; set;}
        public ICollection<TicketAssign> TicketAssigns {get; set;}
        public Stat Status {get; set;}
        public Sender Senders {get; set;}
        public App Apps {get; set;}
        public Module Modules {get; set;}

        public ICollection<Media> Medias {get; set;}

    }
}