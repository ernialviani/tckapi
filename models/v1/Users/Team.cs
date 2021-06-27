using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TicketingApi.Models.v1.Users
{
     
    public class Team
    {
        public int Id {get; set;}
        public string Name {get;set;}
        public string Desc {get;set;}

        public int LeaderId {get; set;}

        [ForeignKey("LeaderId")]
        public User Leader {get;set;}

        public DateTime? CreatedAt {get;  set;}
        public DateTime? UpdatedAt {get; set;}

        public ICollection<TeamDetail> TeamDetails {get; set;}
        
    }
}