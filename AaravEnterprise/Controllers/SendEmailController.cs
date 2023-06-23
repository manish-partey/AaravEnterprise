using AaravEnterprise.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public SendEmailController(EmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;

        }

        public async Task<IActionResult> SendEmail(string name, string email, string subject, string emailBody)
        {
            string toEmail = email;
            string subj = subject;
            string message = emailBody;
            string toUser = name;
            await _emailService.SendEmailAsync(toUser, toEmail, subj, message);
            TempData["success"] = "Thank you, We you get back to you shortly !";
            return RedirectToAction("Contact", "Home");
        }

    }
}
