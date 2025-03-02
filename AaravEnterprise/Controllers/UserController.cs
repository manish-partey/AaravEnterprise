using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var totalRecords = _dbContext.ApplicationUser.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page < 1)
                page = 1;
            if (page > totalPages)
                page = totalPages;

            var userList = _dbContext.ApplicationUser
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(userList);
        }


        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Edit(string? Id)
        {            
            if (Id == null || string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            ApplicationUser? userFromDb = _dbContext.ApplicationUser.Find(Id);

            if (userFromDb == null)
            {
                return NotFound();
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(userFromDb);
        }
        [HttpPost]
        public IActionResult Edit(ApplicationUser obj, string Id)
        {
            if (Id != obj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var applicationUser = _dbContext.ApplicationUser.Find(Id);
                if (applicationUser == null)
                {
                    return NotFound();
                }

                applicationUser.Name = obj.Name;
                applicationUser.Email = obj.Email;
                applicationUser.PhoneNumber = obj.PhoneNumber;
                applicationUser.StreetAddress = obj.StreetAddress;
                applicationUser.City = obj.City;
                applicationUser.State = obj.State;
                applicationUser.PostalCode = obj.PostalCode;

                // Save changes to the database
                _dbContext.SaveChanges();
                TempData["success"] = "User updated successfully";
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public  IActionResult ChangePassword(string? Email)
        {

            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View(model);
            }
            else
            {
                model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                TempData["PasswordChanged"] = "User Password Changed Successfully !";
                return RedirectToAction("Index");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(model);
        }
    }
}
