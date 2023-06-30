using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Cart cart;
        public CheckOutController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return NotFound();
            }
            else
            {
                IQueryable<CartViewModel> query = from C in _dbContext.Cart
                                                  join S in _dbContext.Services on C.ServiceId equals S.Id
                                                  join P in _dbContext.Package on C.PackageId equals P.Id
                                                  where C.ApplicationUserId == userId
                                                  select new CartViewModel
                                                  {
                                                      CartId = C.CartId,
                                                      ServiceId = S.Id,
                                                      ServiceTitle = S.ServiceTitle,
                                                      PackageTitle = P.PackageTitle,
                                                      Amount = C.Amount
                                                  };
                System.Collections.Generic.List<CartViewModel> cart = query.ToList();
                int total = query.Sum(p => p.Amount);
                ViewBag.Total = total; //
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View(cart);
            }
        }
    }
}
