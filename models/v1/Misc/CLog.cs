using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Models.v1.Misc
{
    public class CLogscs
    {
        public int Id {get; set;}
        public string Version {get; set;}
        public string Desc {get; set;}

        public int UserId {get; set;}
        public int AppId {get; set;}
        public int ModuleId {get; set;}

        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  
        
    }
}