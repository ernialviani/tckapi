using System.Collections.Generic;
namespace TicketingApi.Models.v1.Misc
{
    public class Team
    {
        public int Id {get; set;}
        public string Name {get;set;}
        public string Desc {get;set;}

        public int ManagerId {get; set;}

        public ICollection<TeamDetail> TeamDetails {get; set;}
        
    }
}