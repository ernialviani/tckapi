
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
using System;
using System.Text;


namespace TicketingApi.Utils
{
    public class MailUtil: IMailUtil
    {
        private readonly MailSetting _mailSettings;
        public MailUtil(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public string GenerateRandom4Code(){
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[4];
            var random = new Random();
            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }
            var resultString = new String(Charsarr);
            return resultString;
        }

        public async Task SendEmailAsync(MailType mailType)
        {

            string FilePath = Directory.GetCurrentDirectory() + "\\Media\\MailTemplate\\NewTicketHtmlTemplate.html";
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
           // smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
           
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailWelcomeAsync(MailType mailType)
        {

            string FilePath = Directory.GetCurrentDirectory() + "\\Medias\\MailTemplate\\WelcomeHT.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$name", mailType.UserFullName).Replace("$email", mailType.WelcomeEmail).Replace("$pass", mailType.WelcomePass);
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
           // smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


        public async Task SendEmailPostCommentForClientAsync(MailType mailType)
        {

            string FilePath = Directory.GetCurrentDirectory() + "\\Medias\\MailTemplate\\PostCommentForClientHT.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$title", mailType.Title).Replace("$from", mailType.TicketFrom);
            MailText = MailText.Replace("$body", mailType.Body).Replace("$app", mailType.TicketApp).Replace("$module", mailType.TicketModule);
            MailText = MailText.Replace("$user", mailType.UserFullName);
            MailText = MailText.Replace("$linkbutton", mailType.ButtonLink);

            if(!string.IsNullOrEmpty(mailType.VerificationCode)){
                MailText = MailText.Replace("$verifycode", mailType.VerificationCode);
                MailText = MailText.Replace("$descverifycode", mailType.DescVerificationCode);
            }
            else{
                MailText = MailText.Replace("$verifycode", "");
                MailText = MailText.Replace("$descverifycode", "");
            }

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
           // smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailPostCommentAsync(MailType mailType)
        {
            try
            {
                   string FilePath = Directory.GetCurrentDirectory() + "\\Medias\\MailTemplate\\PostCommentHT.html";
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
                // smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
            }
            catch (System.Exception)
            {
                
                throw;
            }
          
       }



        public async Task SendEmailVerificationCodeAsync(MailType mailType)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Medias\\MailTemplate\\VerifiCodeHT.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("$title", mailType.Title).Replace("$body", mailType.Body);
            MailText = MailText.Replace("$code", mailType.VerificationCode);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            foreach (var to in mailType.ToEmail) { email.To.Add(MailboxAddress.Parse(to)); }
            email.Subject = mailType.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}