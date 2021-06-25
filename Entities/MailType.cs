using System.Collections.Generic;  
using Microsoft.AspNetCore.Http;
namespace TicketingApi.Entities
{
    public class MailType
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}