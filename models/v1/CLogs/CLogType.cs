using System;  
using System.Collections.Generic;  

namespace TicketingApi.Models.v1.CLogs
{
    public class CLogType
    {
        public int Id {get; set;}
        public string Name {get;set;}
        public string Color {get;set;}
        public string Desc{get;set;}  

        public ICollection<CLogDetail> CLogDetails {get;set;}

    }
}