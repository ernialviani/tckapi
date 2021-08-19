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

        public int ManagerId {get; set;}

     //   [ForeignKey("ManagerId")]
        public User Manager {get;set;}

        public DateTime? CreatedAt {get;  set;}
        public DateTime? UpdatedAt {get; set;}

        public ICollection<TeamMember> TeamMembers {get; set;}
        
    }
}