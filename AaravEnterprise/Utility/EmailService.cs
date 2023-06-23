using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AaravEnterprise.Utility
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task SendEmailAsync(string toUser, string toEmail, string subject, string message)
        {
            var fromAddress = new MailAddress(_smtpUsername);
            var toAddress = new MailAddress(toEmail);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = _smtpServer;
                smtpClient.Port = _smtpPort;                
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                StringBuilder emailBody = new StringBuilder();
                emailBody.Append("Hello " + toUser);
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append(message);
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("Thank You.");
                emailBody.Append("<br/>");
                emailBody.Append("Aarav Enterprise");
                emailBody.Append("<br/>");
                using (var mailMessage = new MailMessage(fromAddress, toAddress))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = emailBody.ToString();
                    mailMessage.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}