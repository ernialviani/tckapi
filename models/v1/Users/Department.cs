using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  


namespace TicketingApi.Models.v1.Users
{
    public class Department
    {
        public int Id { get; set;}  
        public string Name { get; set;}  
        public string Desc { get; set;} 
        public ICollection<UserDept> UserDepts {get; set;}
    }
}