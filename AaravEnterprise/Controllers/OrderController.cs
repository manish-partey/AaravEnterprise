using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly string query = "";
        private readonly ApplicationDbContext _dbContext;
        public OrderController(ApplicationDbContext dbContext)
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
                System.Collections.Generic.List<Order> OrderForUser = _dbContext.Order.Where(s => s.UserId == userId).ToList();
                ViewBag.UserOrder = OrderForUser;

                //var query = from C in _dbContext.OrderDetails
                //            join S in _dbContext.Services on C.ServiceId equals S.Id                            
                //            where C.OrderId == OrderForUser.Id
                //            select new OrderDetailViewModel
                //            {   
                //                ServiceTitle = S.ServiceTitle,
                //                Amount = C.Price,
                //                Quantity = C.Quantity
                //            };
                //var UserOrderDetails = query.ToList();
                //ViewBag.UserOrderDetails = UserOrderDetails;
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View();
            }

        }
    }
}
