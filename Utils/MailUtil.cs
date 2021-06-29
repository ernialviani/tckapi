
using MailKit.Net.Smtp;
using MailKit.Security;
using TicketingApi.Utils;
using TicketingApi.Entities;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Threading.Tasks;

namespace TicketingApi.Utils
{
    public class MailUtil: IMailUtil
    {
          private readonly MailSetting _mailSettings;
        public MailUtil(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailType mailType)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailType.ToEmail));
            email.Subject = mailType.Subject;
            var builder = new BodyBuilder();
            if (mailType.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailType.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailType.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
    }
    }
}