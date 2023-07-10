using AaravEnterprise.Utility;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AaravEnterprise.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly CustomEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private string toEmail, fromEmail, subj;
        private StringBuilder MessageBody = new StringBuilder();
        public SendEmailController(CustomEmailSender emailSender, IConfiguration configuration, IDNTCaptchaValidatorService dNTCaptchaValidatorService )
        {
            _emailSender = emailSender;
            _configuration = configuration;
            DNTCaptchaValidatorService = dNTCaptchaValidatorService;
        }

        public IDNTCaptchaValidatorService DNTCaptchaValidatorService { get; }

        //[ValidateDNTCaptcha(ErrorMessage ="Please enter correct security code", CaptchaGeneratorLanguage =Language.English, CaptchaGeneratorDisplayMode =DisplayMode.ShowDigits)]
        public IActionResult SendEmail(string name, string email, string phone, string emailBody)
        {
            if (DNTCaptchaValidatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
            {

                toEmail = "support@araventerprise.com";
                fromEmail = email;
                MessageBody.Append("Name : " + name + "<br/>");
                MessageBody.Append("Email : " + email + "<br/>");
                MessageBody.Append("Phone : " + phone + "<br/>");
                MessageBody.Append(emailBody);
                _emailSender.SendEmail(toEmail, "Product Enquiry by " + name, MessageBody.ToString());
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
            else
            {
                TempData["WrongCaptcha"] = "Please enter correct security code!";
                return RedirectToAction("Contact", "Home");
            }
        }

       // [ValidateDNTCaptcha(ErrorMessage = "Please enter correct security code", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult SendEmailContact(string name, string email, string subject, string emailBody)
        {
            if (DNTCaptchaValidatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
            {
                toEmail = "support@araventerprise.com";
                MessageBody.Append("Name : " + name + "<br/>");
                MessageBody.Append("Email : " + email + "<br/>");
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
            else
            {
                TempData["WrongCaptcha"] = "Please enter correct security code!";
                return RedirectToAction("Contact", "Home");
            }
        }


    }
}
