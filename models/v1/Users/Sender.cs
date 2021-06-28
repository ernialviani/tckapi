using System;
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Models.v1.Users
{
    public class Sender
    {

         public int Id { get; set;}  
        public string FirstName { get; set;}  
        public string LastName { get; set;}  
        public string Email { get; set;}  
        public string Password { get; set;}  
        public string Salt { get; set; }
        public IFormFile File {get; set;}
        public string Image {get; set;}
        public bool? LoginStatus {get; set;}
        public DateTime? CreatedAt { get; set;}  
        public DateTime? UpdatedAt { get; set;}  
        
    }
}