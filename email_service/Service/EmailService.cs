using email_service.Contracts;
using System.Net.Mail;
using System.Net;
using email_service.Models;

namespace email_service.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailService(EmailConfig email)
        {
            _emailConfig = email;
        }
        public async Task SendEmailAsync(EmailModel email)
        {
            try
            {
                using (var message = CreateEmailMessage(email.Recipients, email.Subject,email.Body, email.isHtml))
                using (var smtpClient = CreateSmtpClient())
                {
                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendWithAttachmentAsync(EmailModel email, List<IFormFile> files)
        {
            try
            {
                using (var message = CreateEmailMessage(email.Recipients, email.Subject, email.Body, email.isHtml))
                {
                    files.ForEach(async (x) => {
                        using (var stream = x.OpenReadStream())
                        {
                            var memoryStream = new MemoryStream();
                            await stream.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;

                            var attachmentName = x.FileName;
                            var attachmentContentType = x.ContentType;
                            var attachmentContent = new Attachment(memoryStream, attachmentName, attachmentContentType);
                            message.Attachments.Add(attachmentContent);
                        }
                    });

                    using (var smtpClient = CreateSmtpClient())
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private MailMessage CreateEmailMessage(List<string> recipient, string subject, string body, bool isHtml)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_emailConfig.SenderEmail, _emailConfig.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            recipient.ForEach(x => message.To.Add(x));
            
            return message;
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = _emailConfig.SmtpServer,
                Port = _emailConfig.Port,
                EnableSsl = _emailConfig.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
                Timeout = _emailConfig.Timeout
            };
        }
    }
}
