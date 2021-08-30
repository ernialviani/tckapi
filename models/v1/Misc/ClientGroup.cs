using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Models.v1.Misc
{
    public class ClientGroup
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Domain {get; set;}
        public string Desc {get; set;}

        public ICollection<ClientDetail> ClientDetails {get; set;}
    }
}