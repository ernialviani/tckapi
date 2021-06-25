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
    }
}