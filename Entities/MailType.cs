using System;
using System.Collections.Generic;  
using Microsoft.AspNetCore.Http;
namespace TicketingApi.Entities
{
    public class MailType
    {

        public string HomeSite {get;set;}
        public List<string> ToEmail { get; set; }

        public List<string> CCMail {get;set;}
        public string Subject { get; set; }

        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string TicketFrom {get; set;}
        public string TicketApp {get; set;}
        public string TicketModule {get;set;}
        public List<IFormFile> Attachments { get; set; }

        public List<string> AttachmentsString {get; set;}

        public string UserFullName {get; set;}

        public string ButtonLink {get; set;}

        public string WelcomeEmail {get; set;}
        public string WelcomePass {get;set;}

        //verification code

        public string DescVerificationCode {get; set;}
        public string VerificationCode {get; set;}
        

    }
}