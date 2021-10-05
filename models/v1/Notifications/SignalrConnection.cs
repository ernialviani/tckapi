using System.Collections;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;


namespace TicketingApi.Models.v1.Notifications
{
    public class SignalrConnection
    {
        public int Id {get; set;}
        public string ConnectionId {get;set;}
        public int UserId {get;set;}
        public Boolean? Connected {get;set;} 
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  

        
    }
}