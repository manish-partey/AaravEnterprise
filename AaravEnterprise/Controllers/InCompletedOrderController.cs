using AaravEnterprise.DataAccess;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AaravEnterprise.Controllers
{
    public class InCompletedOrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public InCompletedOrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var query = (from u in _dbContext.ApplicationUser
                         join c in _dbContext.Cart on u.Id equals c.ApplicationUserId
                         join o in _dbContext.Order on u.Id equals o.UserId
                         join i in _dbContext.Invoice on o.Id equals i.OrderId
                         where o.PaymentStatus == "InCompleted"
                         group new { u, i, o } by new
                         {
                             UserId = u.Id,
                             u.Name,
                             u.Email,
                             u.PhoneNumber,
                             InvoiceId = i.InvoiceId,
                             OrderId = o.Id,
                             o.TotalAmount
                         } into grouped
                         orderby grouped.Max(g => g.i.InvoiceDate) descending
                         select new UserOrderViewModel
                         {
                             UserId = grouped.Key.UserId,
                             CustName = grouped.Key.Name,
                             CustEmail = grouped.Key.Email,
                             CustPhoneNumber = grouped.Key.PhoneNumber,
                             MaxInvoiceDate = grouped.Max(g => g.i.InvoiceDate),
                             InvoiceId = grouped.Key.InvoiceId,
                             Id = grouped.Key.OrderId,
                             TotalAmount = grouped.Key.TotalAmount
                         }).ToList();




            var paginatedResult = query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)query.Count() / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(paginatedResult);
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
