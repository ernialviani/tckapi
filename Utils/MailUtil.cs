
using System.Data.SqlTypes;
using MailKit.Net.Smtp;
using MailKit.Security;
using TicketingApi.Utils;
using TicketingApi.Entities;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Web;
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

            string FilePath = Directory.GetCurrentDirectory() + "\\Utils\\MailTemplate\\NewTicketHtmlTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$title", mailType.Title).Replace("$body", mailType.Body);
            MailText = MailText.Replace("$from", mailType.TicketFrom).Replace("$app", mailType.TicketApp).Replace("$module", mailType.TicketModule);
            MailText = MailText.Replace("$linkbutton", mailType.ButtonLink);

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            foreach (var to in mailType.ToEmail) {
                 email.To.Add(MailboxAddress.Parse(to));
            }

            if(mailType.CCMail != null){
                 foreach (var cc in mailType.CCMail) {
                      email.Cc.Add(MailboxAddress.Parse(cc));
                 }
            }
            
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
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailPostCommentForClientAsync(MailType mailType)
        {

            string FilePath = Directory.GetCurrentDirectory() + "\\Utils\\MailTemplate\\PostCommentForClientHT.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$title", mailType.Title).Replace("$from", mailType.TicketFrom);
            MailText = MailText.Replace("$body", mailType.Body).Replace("$app", mailType.TicketApp).Replace("$module", mailType.TicketModule);
            MailText = MailText.Replace("$user", mailType.UserFullName);
            MailText = MailText.Replace("$linkbutton", mailType.ButtonLink);

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            foreach (var to in mailType.ToEmail) {
                 email.To.Add(MailboxAddress.Parse(to));
            }

            if(mailType.CCMail != null){
                 foreach (var cc in mailType.CCMail) {
                      email.Cc.Add(MailboxAddress.Parse(cc));
                 }
            }
            
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

            if(mailType.AttachmentsString != null){
                foreach (var file in mailType.AttachmentsString)
                {
                    if (file.Length > 0)
                    {
                            byte[] bytes = System.IO.File.ReadAllBytes(file);
                            var mimeType  = MimeKit.MimeTypes.GetMimeType(file);
                         //   FileStream fs = System.IO.File.Open(file, FileMode.Open);
                            builder.Attachments.Add(Path.GetFileName(file), bytes, ContentType.Parse(mimeType));
                    }
                }
            }
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailPostCommentAsync(MailType mailType)
        {

            string FilePath = Directory.GetCurrentDirectory() + "\\Utils\\MailTemplate\\PostCommentHT.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$title", mailType.Title).Replace("$from", mailType.TicketFrom);
            MailText = MailText.Replace("$body", mailType.Body).Replace("$app", mailType.TicketApp).Replace("$module", mailType.TicketModule);
            MailText = MailText.Replace("$user", mailType.UserFullName);
            MailText = MailText.Replace("$linkbutton", mailType.ButtonLink);

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            foreach (var to in mailType.ToEmail){
                 email.To.Add(MailboxAddress.Parse(to));
            }

            if(mailType.CCMail != null){
                 foreach (var cc in mailType.CCMail) {
                      email.Cc.Add(MailboxAddress.Parse(cc));
                 }
            }
            
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
            if(mailType.AttachmentsString != null){
                foreach (var file in mailType.AttachmentsString)
                {
                    if (file.Length > 0)
                    {
                            byte[] bytes = System.IO.File.ReadAllBytes(file);
                            var mimeType  = MimeKit.MimeTypes.GetMimeType(file);
                         //   FileStream fs = System.IO.File.Open(file, FileMode.Open);
                            builder.Attachments.Add(Path.GetFileName(file), bytes, ContentType.Parse(mimeType));
                    }
                }
            }
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
       }
    }
}