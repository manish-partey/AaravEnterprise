using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class CompletedOrderController : Controller
    {        
        private readonly ApplicationDbContext _dbContext;
        public CompletedOrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {   
                System.Collections.Generic.List<Order> CompletedOrder = _dbContext.Order.ToList();
                ViewBag.UserOrder = CompletedOrder;                
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View();
        }
    }
}
