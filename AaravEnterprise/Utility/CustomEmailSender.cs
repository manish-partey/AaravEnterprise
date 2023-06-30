using AaravEnterprise.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace AaravEnterprise.Utility
{
    public class CustomEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        public CustomEmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_smtpSettings.Username);
                mailMessage.To.Add(toAddress);
                mailMessage.CC.Add(_smtpSettings.Username);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.Port))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}