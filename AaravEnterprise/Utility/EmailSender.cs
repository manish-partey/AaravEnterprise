using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AaravEnterprise.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly CustomEmailSender _emailSender;
        public IConfiguration Configuration { get; }
        public EmailSender()
        {

        }
        public EmailSender(IConfiguration configuration, CustomEmailSender emailSender)
        {
            _emailSender = emailSender;
            Configuration = configuration;

        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _emailSender.SendEmail(email, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}
