using System;
using System.Collections.Generic;  
using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Users;

namespace TicketingApi.Entities
{
    public class NotifType
    {
        public const string TICKET_CREATE = "TICKET_CREATE";
        public const string TICKET_ASSIGN = "TICKET_ASSIGN";
        public const string TICKET_STATUS = "TICKET_STATUS";
        public const string TICKET_RESPONSE = "TICKET_RESPONSE";
    }

    public class NotifSetting 
    {
        public string NotifType {get;set;}
        public string Title {get; set;}
        public string Message {get; set; }
        public string LinkAction {get; set;}
        public List<User> Users {get; set;}
        public List<Sender> Senders {get;set;}
    }
}