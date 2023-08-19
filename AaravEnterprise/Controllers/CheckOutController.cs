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

                        _dbContext.Order.Add(FinalOrder);
                        _dbContext.SaveChanges();
                        int orderID = FinalOrder.Id;

                        OrderDetails = new OrderDetails();

                        foreach (CartViewModel cartItem in cart)
                        {
                            int orderId = orderID;
                            int serviceId = cartItem.ServiceId;
                            int quantity = 1;
                            decimal price = cartItem.Amount;
                             total = cartItem.Amount;

                            string sqlquery = $@"SET IDENTITY_INSERT [OrderDetails] OFF;
                                        INSERT INTO [OrderDetails]
                                        ([OrderId]
                                        ,[ServiceId]
                                        ,[Quantity]
                                        ,[Price]
                                        ,[Total])
                                VALUES
                                        ({orderId}
                                        ,{serviceId}
                                        ,{quantity}
                                        ,{price}
                                        ,{total})

                                         SET IDENTITY_INSERT [OrderDetails] ON;";
                            int result = _dbContext.Database.ExecuteSqlRaw(sqlquery);
                        }

                        Invoice = new Invoice()
                        {
                            OrderId = orderID,
                            InvoiceDate = System.DateTime.Now,
                            Amount = total
                        };
                        _dbContext.Invoice.Add(Invoice);
                        _dbContext.SaveChanges();

                        //foreach (CartViewModel cartItem in cart)
                        //{
                        //    Cart rowToRemove = _dbContext.Cart.Find(cartItem.CartId);
                        //    if (rowToRemove != null)
                        //    {
                        //        _dbContext.Cart.Remove(rowToRemove);
                        //    }
                        //}
                        //_dbContext.SaveChanges();
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
