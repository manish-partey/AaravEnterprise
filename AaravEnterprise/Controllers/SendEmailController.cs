using AaravEnterprise.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AaravEnterprise.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly CustomEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private string toEmail;
        private string subj;
        private StringBuilder MessageBody = new StringBuilder();
        public SendEmailController(CustomEmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public IActionResult SendEmail(string name, string email, string subject, string emailBody)
        {
            toEmail = email;
            subj = subject;

            MessageBody.Append(emailBody);

            _emailSender.SendEmail("support@araventerprise.com", subject, MessageBody.ToString());
            MessageBody = new StringBuilder();
            MessageBody.Append("Dear " + name);
            MessageBody.Append("<br />");
            MessageBody.Append("Thank you for your Email");
            MessageBody.Append("<br />");
            MessageBody.Append("We you get back to you shortly !");
            MessageBody.Append("<br />");
            MessageBody.Append("Thank You");
            MessageBody.Append("<br />");
            MessageBody.Append("Aarav Enterprise");
            _emailSender.SendEmail(email, "Automatic Reply - Please do not reply", MessageBody.ToString());
            TempData["success"] = "Thank you, We you get back to you shortly !";
            return RedirectToAction("Contact", "Home");
        }
    }
}
