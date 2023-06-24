using System.Net.Mail;
using AaravEnterprise.Models;
using Microsoft.Extensions.Options;

namespace AaravEnterprise.Utility
{
    public class EmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_smtpSettings.Username);
                mailMessage.To.Add(toAddress);
                mailMessage.CC.Add(_smtpSettings.Username);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.Port))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}