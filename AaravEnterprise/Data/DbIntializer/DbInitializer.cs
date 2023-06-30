using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AaravEnterprise.Data.DbIntializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }
        public void Initialize()
        {
            try
            {
                if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }
            }
            catch (Exception) { }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            }

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@aaravwebsolution.com",
                Email = "admin@aaravwebsolution.com",
                Name = "Manish Partey",
                PhoneNumber = "7400041113",
                StreetAddress = "Building no 8/380 GTB Nagar sion Koliwada  ",
                State = "Maharastra",
                PostalCode = "400037",
                City = "Mumbai"
            }, "InfoInfo@2023").GetAwaiter().GetResult();

            ApplicationUser user = _applicationDbContext.ApplicationUser.FirstOrDefault(u => u.Email == "admin@aaravwebsolution.com");
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            return;
        }
    }
}
