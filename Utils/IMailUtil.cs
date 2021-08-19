using TicketingApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingApi.Utils
{
    public interface IMailUtil
    {
           Task SendEmailAsync(MailType mailType);

            Task SendEmailWelcomeAsync(MailType mailType);
           Task SendEmailPostCommentAsync(MailType mailType);
           Task SendEmailPostCommentForClientAsync(MailType mailType);
           
    }
}