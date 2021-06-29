using System;

namespace TicketingApi.Models.v1.Users
{
    public class UserRole
    {
        public int Id { get; set;}  
        public int UserId { get; set;}  

       //  public User Users {get; set;}
        public int RoleId { get; set;}  
         public Role Roles {get; set;}
        
    }
}