using System;

namespace TicketingApi.Models.v1.Users
{
    public class UserDept
    {
        public int Id { get; set;}  
        public int UserId { get; set;}

       public User Users {get; set;}

        public int DepartmentId { get; set;}  
        
        public Department Departments {get; set;}

    }
}