using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
  
namespace TicketingApi.Models.v1.Users
{  
    public class User  
    {  
        public int Id { get; set;}  
        public string FirstName { get; set;}  
        public string LastName { get; set;}  
        public string Email { get; set;}  
        public string Password { get; set;}  
        public string Salt { get; set; }
        public string Image {get; set;}
        public DateTime CreationDateTime { get; set;}  
        public DateTime? LastUpdateDateTime { get; set;}  

        public ICollection<UserRole> UserRoles {get; set;}
        public ICollection<UserDept> UserDepts {get; set;}
    }  
} 