using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            // Filter completed orders and count the total number of completed orders
            var completedOrders = _dbContext.Order
                                            .Where(o => o.OrderStatus == "Completed").OrderByDescending(o => o.OrderDate)
                                            .ToList();
            var totalCompletedOrders = completedOrders.Count();

            // Paginate the completed orders based on the page and pageSize parameters
            var paginatedCompletedOrders = completedOrders.Skip((page - 1) * pageSize)
                                                         .Take(pageSize)
                                                         .ToList();

            // Pass the paginated orders and pagination metadata to the view
            ViewBag.UserOrder = paginatedCompletedOrders;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCompletedOrders / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

    }
}
