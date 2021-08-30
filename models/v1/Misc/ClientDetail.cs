using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Models.v1.Misc
{
    public class ClientDetail
    {
      public int Id {get; set;}
      public int ClientGroupId {get;set;}
      public string Name {get; set;}
      public string Domain {get; set;}

      public string Desc {get; set;}

      public ClientGroup ClientGroup {get;set;}


    }
}