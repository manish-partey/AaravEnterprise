using AaravEnterprise.Models;
using AaravEnterprise.Utility;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AaravEnterprise.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly CountryService _countryService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IReCaptchaService _reCaptchaService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IReCaptchaService reCaptchaService,
            CountryService countryService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _reCaptchaService = reCaptchaService;
            _countryService = countryService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public List<Country> countries { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [RegularExpression(@"^\d{10}$|^\d+$", ErrorMessage = "Please enter a valid phone number or a numeric value.")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }
            [Display(Name = "Street Address")]
            public string? StreetAddress { get; set; }
            [Display(Name = "City")]
            public string? City { get; set; }
            [Display(Name = "State")]
            public string? State { get; set; }
            [Display(Name = "Postal Code")]
            public string? PostalCode { get; set; }

            [Required]
            [RequiredIfNotSelectCountry("Country")]
            [Display(Name = "Country")]
            public string Country { get; set; }


            [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
            public class RequiredIfNotSelectCountryAttribute : ValidationAttribute
            {
                private readonly string _comparisonProperty;

                public RequiredIfNotSelectCountryAttribute(string comparisonProperty)
                {
                    _comparisonProperty = comparisonProperty;
                }

                protected override ValidationResult IsValid(object value, ValidationContext validationContext)
                {
                    var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
                    if (comparisonProperty == null)
                    {
                        throw new ArgumentException($"Property {_comparisonProperty} not found.");
                    }

                    var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance, null);

                    if (comparisonValue != null && string.Equals(comparisonValue.ToString(), "Select Country", StringComparison.OrdinalIgnoreCase))
                    {
                        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            return new ValidationResult(ErrorMessage);
                        }
                    }

                    return ValidationResult.Success;
                }
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            countries = _countryService.GetAllCountries();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Request.Form["g-recaptcha-response"]))
                {
                    ModelState.AddModelError(string.Empty, "Please Validate Captcha!");
                    return Page();
                }
                ApplicationUser user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.PhoneNumber, Name = Input.Name, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, PostalCode = Input.PostalCode, Country=Input.Country };
                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
