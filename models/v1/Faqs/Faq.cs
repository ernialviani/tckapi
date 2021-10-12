using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Faqs;
using TicketingApi.Models.v1.Misc;

namespace TicketingApi.Models.v1.Faqs
{
    public class Faq
    {
         public int Id {get; set;}
        public string Question {get; set;}
        public string Desc {get; set;}

        public int UserId {get; set;}
        public int AppId {get; set;}
        public int ModuleId {get; set;}

        public User Users {get;set;}
        public App Apps {get;set;}
        public Module Modules {get;set;}

        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  
    }
}