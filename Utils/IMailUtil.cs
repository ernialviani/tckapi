using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketingApi.Entities;

namespace TicketingApi.Utils
{
    public interface IMailUtil
    {
        string GenerateRandom4Code();
        Task SendEmailAsync(MailType mailType);
        Task SendEmailWelcomeAsync(MailType mailType);
        Task SendEmailPostCommentAsync(MailType mailType);
        Task SendEmailPostCommentForClientAsync(MailType mailType);
        Task SendEmailVerificationCodeAsync(MailType mailType);
           
    }
}