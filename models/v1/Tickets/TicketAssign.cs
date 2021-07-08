using System;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingApi.Models.v1.Tickets
{
    public class TicketAssign
    {
        public int Id {get;set;}
        public int TicketId {get; set;}
         public int? TeamId {get; set;}
         public DateTime? TeamAt {get; set;}
         public int? UserId {get; set;}
         public DateTime? UserAt {get; set;}

         public User Users {get; set;}

         public Team Teams {get; set;}

         public string AssignType {get; set;} //M MANAGER //T TEAM //U USER

        public bool Viewed {get; set;}
        public DateTime? ViewedAt {get; set;}
    

    }
}