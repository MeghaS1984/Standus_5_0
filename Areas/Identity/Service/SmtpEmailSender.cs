using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Standus_5_0.Areas.Identity.Service
{

    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpEmailSender(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }

}
