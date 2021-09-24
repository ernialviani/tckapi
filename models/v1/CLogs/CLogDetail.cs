using System;  
using System.Collections.Generic;  
using TicketingApi.Models.v1.Misc;

namespace TicketingApi.Models.v1.CLogs
{
    public class CLogDetail
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string Desc {get;set;}
        public int CLogId {get; set;}
        public int CLogTypeId {get; set;}
        public int? ModuleId {get; set;}

        public CLog CLogs {get; set;}
        public CLogType CLogTypes {get;set;}
        public Module Modules {get;set;}
        public ICollection<Media> Medias {get; set;}
        
        //post purpose
        public ICollection<string> ImageName {get;set;}
    }
}