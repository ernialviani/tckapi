using System.Collections;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;


namespace TicketingApi.Models.v1.CLogs
{
    public class CLog
    {
        public int Id {get; set;}
        public string Version {get; set;}
        public string Desc {get; set;}

        public int UserId {get; set;}
        public int AppId {get; set;}
     
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  

        public User Users {get; set;}
        public App Apps {get; set;}

        public ICollection<CLogDetail> CLogDetails {get; set;}
        
    }
}