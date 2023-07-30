using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AaravEnterprise.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            List<ApplicationUser> objUserList = _dbContext.ApplicationUser.ToList();
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(objUserList);
        }
        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
