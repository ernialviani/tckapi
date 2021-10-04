using System.Collections;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;


namespace TicketingApi.Models.v1.Notifications
{
    public class Notif
    {
        public int Id {get; set;}

        public int NotifRegisterId {get;set;}
        public bool Viewed {get;set;}
        
        public string Title {get;set;}
        public string Message {get; set;}
        public string Link {get; set;}

        public string NtfType {get; set;}

        public string NtfData {get; set;}
     
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  

        
    }
}