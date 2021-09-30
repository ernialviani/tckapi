using System.Collections;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;


namespace TicketingApi.Models.v1.Notifications
{
    public class NotifRegister
    {
        public int Id {get; set;}
        public int UserId {get;set;}
        public string UserMail {get;set;}
        public string UserToken {get;set;}
        public string Os {get;set;}
        public string OsVersion {get;set;}
        public string Browser {get;set;}
        public string BrowserVersion {get;set;}
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  

        
    }
}