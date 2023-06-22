using AaravEnterprise.DataAccess;
using Microsoft.AspNetCore.Mvc;
using AaravEnterprise.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AaravEnterprise.ViewModel;

namespace AaravEnterprise.Controllers
{[Authorize]
    public class OrderController : Controller
    {
        string query = "";
        private readonly ApplicationDbContext _dbContext;
        public OrderController(ApplicationDbContext dbContext)
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
                var OrderForUser = _dbContext.Order.FirstOrDefault(s => s.UserId == userId);
                ViewBag.UserOrder = OrderForUser;

                var query = from C in _dbContext.OrderDetails
                            join S in _dbContext.Services on C.ServiceId equals S.Id                            
                            where C.OrderId == OrderForUser.Id
                            select new OrderDetailViewModel
                            {   
                                ServiceTitle = S.ServiceTitle,
                                Amount = C.Price,
                                Quantity = C.Quantity
                            };
                var UserOrderDetails = query.ToList();
                ViewBag.UserOrderDetails = UserOrderDetails;
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View();
            }
            
        }
    }
}
