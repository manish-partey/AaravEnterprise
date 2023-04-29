﻿using AaravEnterprise.DataAccess;
using Microsoft.AspNetCore.Mvc;
using AaravEnterprise.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        Cart cart;
        public CheckOutController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return NotFound();
            }
            else
            {
                var query = from C in _dbContext.Cart
                            join S in _dbContext.Services on C.ServiceId equals S.Id
                            join P in _dbContext.Package on C.PackageId equals P.Id
                            where C.ApplicationUserId == userId
                            select new
                            {
                                C.CartId,
                                S.ServiceTitle,
                                P.PackageTitle,
                                C.Amount

                            };
                var cart = query.ToList();
                var total = query.Sum(p => p.Amount);
                ViewBag.Total = total; //
                return View(cart);
            }
        }
    }
}
