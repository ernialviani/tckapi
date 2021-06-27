using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Tickets;

namespace TicketingApi.Models.v1.Misc
{
    public class Media
    {
        public int Id {get; set;}
        public string FileType {get; set;}
        public string FileName {get; set;}

        public IFormFile File {get; set;}
        
        public int RelId {get;set;}
        public string RelType {get; set;}

        // [ForeignKey("RelId")]
        // public Ticket Tickets {get; set;} 

        // [ForeignKey("RelId")]
        // public TicketDetail TicketDetails {get; set;}
    }
}