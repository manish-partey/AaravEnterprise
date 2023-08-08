using AaravEnterprise.DataAccess;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{
    public class InCompletedOrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public InCompletedOrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var query = from user in _dbContext.ApplicationUser
                        join cart in _dbContext.Cart on user.Id equals cart.ApplicationUserId
                        group cart by new
                        {
                            user.Id,
                            user.Name,
                            user.Email,
                            user.PhoneNumber
                        } into grouped
                        select new UserOrderViewModel
                        {
                            UserId = grouped.Key.Id,
                            CustName = grouped.Key.Name,
                            CustEmail = grouped.Key.Email,
                            CustPhoneNumber = grouped.Key.PhoneNumber,
                            TotalAmount = grouped.Sum(c => c.Amount)
                        };
            var result = query.ToList();
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(result);
        }

        public IActionResult OrderDetails(string UserId)
        {
            if (UserId == null)
            {
                return NotFound();
            }
            else
            {
                IQueryable<CartViewModel> query = from C in _dbContext.Cart
                                                  join S in _dbContext.Services on C.ServiceId equals S.Id
                                                  join P in _dbContext.Package on C.PackageId equals P.Id
                                                  where C.ApplicationUserId == UserId
                                                  select new CartViewModel
                                                  {
                                                      CartId = C.CartId,
                                                      ServiceTitle = S.ServiceTitle,
                                                      PackageTitle = P.PackageTitle,
                                                      Amount = C.Amount
                                                  };
                System.Collections.Generic.List<CartViewModel> cart = query.ToList();
                int total = query.Sum(p => p.Amount);
                ViewBag.Total = total;
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View(cart);
            }
        }
    }
}
