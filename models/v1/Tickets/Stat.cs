using System.Collections.Generic;
namespace TicketingApi.Models.v1.Tickets
{
    public class Stat
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Color {get; set;}
        public string Desc {get; set;}

        public ICollection<Ticket> Tickets {get; set;}
    }
}