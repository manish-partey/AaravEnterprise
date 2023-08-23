using AaravEnterprise.DataAccess;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{
    public class ViewInvoiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ViewInvoiceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(int Id)
        {
            var query = from order in _dbContext.Order
                        join invoice in _dbContext.Invoice on order.Id equals invoice.OrderId
                        join user in _dbContext.ApplicationUser on order.UserId equals user.Id
                        join orderDetail in _dbContext.OrderDetails on order.Id equals orderDetail.OrderId
                        join service in _dbContext.Services on orderDetail.ServiceId equals service.Id
                        where invoice.InvoiceId == Id
                        select new InCompleteOrderViewModel
                        {
                            InvoiceId = invoice.InvoiceId,
                            OrderId = order.Id,
                            OrderStatus = order.OrderStatus,
                            PaymentStatus = order.PaymentStatus,
                            Email = user.Email,
                            Name = user.Name,
                            PhoneNumber = user.PhoneNumber,
                            InvoiceDate = invoice.InvoiceDate,
                            Amount = invoice.Amount,
                            ServiceTitle = service.ServiceTitle,
                            Price = orderDetail.Price
                        };

            var result = query.ToList();
            double total = query.Sum(p => p.Price);
            ViewBag.Total = total; //
            var resultItem = query.FirstOrDefault();
            ViewBag.InvoiceId = resultItem.InvoiceId;
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(result);
        }
    }
}
