using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Cart cart;
        private AaravEnterprise.Models.Order FinalOrder;
        private OrderDetails OrderDetails;
        private Invoice Invoice;
        private string sqlquery = "";
        private int result;
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


                //var queryResult = from invoice in _dbContext.Invoice
                //                  join order in _dbContext.Order on invoice.OrderId equals order.Id
                //                  where order.UserId == userId
                //                  select new
                //                  {
                //                      invoice.InvoiceId,
                //                      invoice.InvoiceDate,
                //                      order.Id,
                //                      order.UserId
                //                  };

                //var resultItem = queryResult.FirstOrDefault(); // Get the first (or default) result

                //int InvoiceId = resultItem.InvoiceId; // Access UserId property if result is not null
                
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        FinalOrder = new Models.Order
                        {
                            UserId = userId,
                            OrderDate = DateTime.UtcNow,
                            TotalAmount = total,
                            OrderStatus = "Payment Pending",
                            PaymentStatus = "InCompleted",
                            PaymentDate = DateTime.UtcNow,
                            PaymentId = "NA"
                        };

                        var userOrder = _dbContext.Order.FirstOrDefault(order => order.UserId == userId && order.PaymentStatus == "InCompleted");
                        int orderID;
                        if (userOrder != null)
                        { 
                            orderID = userOrder.Id;
                            FinalOrder = _dbContext.Order.FirstOrDefault(U =>U.Id==orderID);
                            _dbContext.Entry(userOrder).State = EntityState.Detached;

                            if (ModelState.IsValid)
                            {
                                FinalOrder = new Models.Order
                                {
                                    Id = orderID,
                                    UserId = userId,
                                    OrderDate = DateTime.UtcNow,
                                    TotalAmount = total,
                                    OrderStatus = "Payment Pending",
                                    PaymentStatus = "InCompleted",
                                    PaymentDate = DateTime.UtcNow,
                                    PaymentId = "NA"
                                };
                                _dbContext.Order.Update(FinalOrder);
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            _dbContext.Order.Add(FinalOrder);
                            _dbContext.SaveChanges();
                            orderID = FinalOrder.Id;
                        }

                        sqlquery = $@"DELETE FROM [OrderDetails] WHERE OrderId=" + orderID + ";";
                        result = _dbContext.Database.ExecuteSqlRaw(sqlquery);

                        OrderDetails = new OrderDetails();

                        foreach (CartViewModel cartItem in cart)
                        {   
                            int serviceId = cartItem.ServiceId;
                            int quantity = 1;
                            decimal price = cartItem.Amount;

                             sqlquery = $@"SET IDENTITY_INSERT [OrderDetails] OFF;
                                        INSERT INTO [OrderDetails]
                                        ([OrderId]
                                        ,[ServiceId]
                                        ,[Quantity]
                                        ,[Price]
                                        ,[Total])
                                VALUES
                                        ({orderID}
                                        ,{serviceId}
                                        ,{quantity}
                                        ,{price}
                                        ,{price})

                                         SET IDENTITY_INSERT [OrderDetails] ON;";
                            result = _dbContext.Database.ExecuteSqlRaw(sqlquery);
                        }

                        Invoice = new Invoice()
                        {
                            OrderId = orderID,
                            InvoiceDate = System.DateTime.Now,
                            Amount = total
                        };

                         sqlquery = $@"DELETE FROM [Invoice] WHERE OrderId=" + orderID + ";";
                         result = _dbContext.Database.ExecuteSqlRaw(sqlquery);

                        _dbContext.Invoice.Add(Invoice);
                        _dbContext.SaveChanges();

                        ViewBag.InvoiceId = Invoice.InvoiceId;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View(cart);
            }
        }
    }
}
