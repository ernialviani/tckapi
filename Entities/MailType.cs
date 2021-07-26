using System.Collections.Generic;  
using Microsoft.AspNetCore.Http;
namespace TicketingApi.Entities
{
    public class MailType
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public string TicketFrom {get; set;}
        public string TicketApp {get; set;}
        public string TicketModule {get;set;}
        public List<IFormFile> Attachments { get; set; }

        public string UserFullName {get; set;}

        public string ButtonLink {get; set;}
    }
}