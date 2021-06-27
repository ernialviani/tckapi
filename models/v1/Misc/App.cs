using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
namespace TicketingApi.Models.v1.Misc
{
    public class App
    {
        public int Id {get; set;}
        public string Name {get; set;}

        public IFormFile File {get; set;}
        public string Logo {get;set;}
        public string Desc {get;set;}

        public ICollection<Module> Modules {get;set;}
    }
}