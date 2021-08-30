using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Models.v1.Misc
{
    public class Verification
    {
        public int Id {get;set;}
        public string Code {get; set;}
        public Boolean? Verified {get; set;}
        public DateTime? ExpiredAt { get; set;}  
        public int? UserId {get; set;}
        public int? SenderId {get;set;}
        public string Desc {get; set;}
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  
    }
}