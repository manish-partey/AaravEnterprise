﻿using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.Utility;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{  
    public class PaypalPaymentController : Controller
    {
        private readonly CustomEmailSender _emailSender;
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext _dbContext;
        private readonly Cart cart;
        private AaravEnterprise.Models.Order FinalOrder;
        private OrderDetails OrderDetails;
        public PaypalPaymentController(IConfiguration configuration, ApplicationDbContext dbContext, CustomEmailSender emailSender)
        {
            _emailSender = emailSender;
            Configuration = configuration;
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        public async Task<ActionResult> Paypalvtwo(int invoiceID, string Cancel = null)
        {
            string userId = "";
            if (HttpContext.Session.GetString("invoiceID") == null)
                HttpContext.Session.SetString("invoiceID", Convert.ToString(invoiceID));


            if (invoiceID != 0 || HttpContext.Session.GetString("invoiceID")!=null)
            {
                if (invoiceID == 0)
                {
                    invoiceID = Convert.ToInt32(HttpContext.Session.GetString("invoiceID"));
                }

                var queryResult = from invoice in _dbContext.Invoice
                                  join order in _dbContext.Order on invoice.OrderId equals order.Id
                                  where invoice.InvoiceId == invoiceID
                                  select new
                                  {
                                      invoice.InvoiceId,
                                      invoice.InvoiceDate,
                                      order.Id,
                                      order.UserId
                                  };

                var resultItem = queryResult.FirstOrDefault(); // Get the first (or default) result

                userId = resultItem?.UserId; // Access UserId property if result is not null
            }
            else
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            List<CartViewModel> cartViewModels = new List<CartViewModel>();        

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
            ViewBag.CartItemsForUser = query.ToList();
            int total = query.Sum(p => p.Amount);
            ViewBag.Total = total;

            #region local_variables
            MyPaypalPayment.MyPaypalSetup payPalSetup
                = new MyPaypalPayment.MyPaypalSetup { Environment = Configuration.GetSection("PayPal:Mode").Value.ToString(), ClientId = Configuration.GetSection("PayPal:ClientId").Value.ToString(), Secret = Configuration.GetSection("PayPal:Secret").Value.ToString() };

            List<string> paymentResultList = new List<string>();
            #endregion

            #region check_payment_cancellation            
            if (!string.IsNullOrEmpty(Cancel) && Cancel.Trim().ToLower() == "true")
            {
                paymentResultList.Add("You cancelled the transaction.");
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View("PaymentDetails", paymentResultList);
            }

            #endregion

            payPalSetup.PayerApprovedOrderId = Request.Query["token"];
            string PayerID = Request.Query["PayerID"];

            //when payerID is null it means order is not approved by the payer.            
            if (string.IsNullOrEmpty(PayerID))
            {
                #region order_creation
                //Create order and display it to the payer to approve. 
                //This is the first PayPal screen where payer signin using his PayPal credentials
                try
                {
                    //redirect URL. when approved or cancelled on PayPal, PayPal uses this URL to redirect to your app/website.
                    payPalSetup.RedirectUrl = Request.Scheme + "://" + Request.Host + "/PaypalPayment/Paypalvtwo?";
                    MyPaypalPayment myPaypalPayment = new MyPaypalPayment();
                    cartViewModels = (List<CartViewModel>)ViewBag.CartItemsForUser;
                    string ordertotal = Convert.ToString(ViewBag.Total);
                    PayPalHttp.HttpResponse response = await myPaypalPayment.createOrder(payPalSetup, cartViewModels, ordertotal);

                    HttpStatusCode statusCode = response.StatusCode;
                    PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();
                    foreach (PayPalCheckoutSdk.Orders.LinkDescription link in result.Links)
                    {
                        if (link.Rel.Trim().ToLower() == "approve")
                        {
                            payPalSetup.ApproveUrl = link.Href;
                        }
                    }

                    if (!string.IsNullOrEmpty(payPalSetup.ApproveUrl))
                    {
                        return Redirect(payPalSetup.ApproveUrl);
                    }
                }
                catch (Exception ex)
                {
                    paymentResultList.Add("There was an error in processing your payment");
                    paymentResultList.Add("Details: " + ex.Message);
                }

                #endregion
            }
            else
            {
                #region order_execution
                MyPaypalPayment myPaypalPayment = new MyPaypalPayment();
                //this is where actual transaction is carried out
                PayPalHttp.HttpResponse response = await myPaypalPayment.captureOrder(payPalSetup);
                try
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                    //update view bag so user/payer gets to know the status
                    if (result.Status.Trim().ToUpper() == "COMPLETED")
                    {
                        cartViewModels = (List<CartViewModel>)ViewBag.CartItemsForUser;
                    }

                    SendOrderConfirmationEmail(userId, total, result.Id, cartViewModels);
                    CompleteOrder(userId, total, result.Id, cartViewModels);
                    paymentResultList.Add("Payment Successful. Thank you.");
                    paymentResultList.Add("Payment State: " + result.Status);
                    paymentResultList.Add("Payment ID: " + result.Id);

                    #region null_checks
                    if (result.PurchaseUnits != null && result.PurchaseUnits.Count > 0 &&
                        result.PurchaseUnits[0].Payments != null && result.PurchaseUnits[0].Payments.Captures != null &&
                        result.PurchaseUnits[0].Payments.Captures.Count > 0)
                        #endregion
                        paymentResultList.Add("Transaction ID: " + result.PurchaseUnits[0].Payments.Captures[0].Id);
                }
                catch (Exception ex)
                {
                    paymentResultList.Add("There was an error in processing your payment");
                    paymentResultList.Add("Details: " + ex.Message);
                }

                #endregion

            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View("PaymentDetails", paymentResultList);
        }

        public void SendOrderConfirmationEmail(string userId, double orderAmount, string paymentId, List<CartViewModel> cartViewModels)
        {
            var objUser = _dbContext.ApplicationUser.FirstOrDefault(c => c.Id == userId);

            if (objUser == null) return; // Check if user exists

            var emailBody = new StringBuilder()
                .Append("<html>")
                .Append("<head>")
                .Append("<title>Order Confirmation</title>")
                .Append("</head>")
                .Append("<body>")
                .Append("<h1>Order Confirmation</h1>")
                .AppendFormat("<p>Dear {0},</p>", objUser.Name)
                .Append("<p>Thank you for your order! We are pleased to confirm that your order has been received and is being processed.</p>")
                .Append("<h2>Order Details</h2>")
                .Append("<table>")
                .Append("<thead>")
                .Append("<tr>")
                .Append("<th>Service</th>")
                .Append("<th>Package</th>")
                .Append("<th>Amount</th>")
                .Append("</tr>")
                .Append("</thead>")
                .Append("<tbody>");

            foreach (var cartItem in cartViewModels)
            {
                emailBody.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", cartItem.ServiceTitle, cartItem.PackageTitle, cartItem.Amount);
            }

            emailBody
                .Append("</tbody>")
                .Append("</table>")
                .AppendFormat("<p>Total Amount: USD {0}</p>", orderAmount)
                .Append("<h2>Payment Information</h2>")
                .AppendFormat("<p>Payment Reference Number: {0}</p>", paymentId)
                .Append("<p>If you have any questions regarding your order, please feel free to contact our customer support team.</p>")
                .Append("<p>Thank you for shopping with us!</p>")
                .Append("<p>Sincerely,<br> Aarav Enterprise</p>")
                .Append("</body>")
                .Append("</html>");

            _emailSender.SendEmail(objUser.Email, "Payment Confirmation for Aarav Enterprise", emailBody.ToString());
        }

        public void CompleteOrder(string userId, double orderAmount, string paymentId, List<CartViewModel> cartViewModels)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var existingOrder = _dbContext.Order.FirstOrDefault(x => x.UserId == userId && x.PaymentStatus == "InCompleted");
                    if (existingOrder != null)
                    {
                        existingOrder.UserId = userId;
                        existingOrder.OrderDate = DateTime.UtcNow;
                        existingOrder.TotalAmount = orderAmount;
                        existingOrder.OrderStatus = "Completed";
                        existingOrder.PaymentStatus = "Completed";
                        existingOrder.PaymentDate = DateTime.UtcNow;
                        existingOrder.PaymentId = paymentId;

                        _dbContext.SaveChanges();
                    }

                    foreach (CartViewModel cartItem in cartViewModels)
                    {
                        Cart rowToRemove = _dbContext.Cart.Find(cartItem.CartId);
                        if (rowToRemove != null)
                        {
                            _dbContext.Cart.Remove(rowToRemove);
                        }
                    }
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }

    /// <summary>
    /// This is your own custom class. NOT from paypal SDK. Write it the way you want it
    /// </summary>
    public class MyPaypalPayment
    {
        private List<CartViewModel> cartItemList;
        /// <summary>
        /// Initiates Paypal client. Must ensure correct environment.
        /// </summary>
        /// <param name="paypalEnvironment">provide value sandbox for testing,  provide value live for live environments</param>            
        /// <returns>PayPalHttp.HttpClient</returns>
        public PayPalHttpClient client(MyPaypalSetup paypalEnvironment)
        {
            PayPalEnvironment environment = null;

            if (paypalEnvironment.Environment == "live")
            {
                // Creating a live environment
                environment = new LiveEnvironment(paypalEnvironment.ClientId, paypalEnvironment.Secret);
            }
            else
            {
                // Creating a sandbox environment
                environment = new SandboxEnvironment(paypalEnvironment.ClientId, paypalEnvironment.Secret);
            }

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }

        //### Creating an Order
        //This will create an order and print order id for the created order

        public async Task<PayPalHttp.HttpResponse> createOrder(MyPaypalSetup paypalSetup, List<CartViewModel> cartViewModels, string orderTotal)
        {
            PayPalHttp.HttpResponse response = null;
            cartItemList = cartViewModels;
            List<Item> itemList = new List<Item>();
            foreach (CartViewModel cartItem in cartItemList)
            {
                itemList.Add(new Item
                {
                    Quantity = "1",
                    Name = cartItem.PackageTitle,
                    Description = cartItem.ServiceTitle,
                    Sku = "sku",
                    Tax = new Money() { CurrencyCode = "USD", Value = "0.00" },
                    UnitAmount = new Money
                    {
                        CurrencyCode = "USD",
                        Value = cartItem.Amount.ToString("0.##")
                    }
                });
            }

            try
            {
                // Construct a request object and set desired parameters
                // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
                #region order_creation
                OrderRequest order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>()
                        {
                            new PurchaseUnitRequest()
                            {
                               Items = itemList,
                               AmountWithBreakdown = new AmountWithBreakdown()
                                {
                                    CurrencyCode = "USD",
                                    Value = orderTotal,

                                    AmountBreakdown = new AmountBreakdown()
                                    {
                                        TaxTotal = new PayPalCheckoutSdk.Orders.Money()
                                        {
                                            CurrencyCode = "USD",
                                            Value = "0.00"
                                        },
                                        ItemTotal = new PayPalCheckoutSdk.Orders.Money()
                                        {
                                            CurrencyCode = "USD",
                                            Value = orderTotal
                                        }
                                    }
                                }
                            }
                        },
                    ApplicationContext = new ApplicationContext()
                    {
                        ReturnUrl = paypalSetup.RedirectUrl,
                        CancelUrl = paypalSetup.RedirectUrl + "&Cancel=true"
                    }
                };

                #endregion

                //IMPORTANT
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Call API with your client and get a response for your call
                OrdersCreateRequest request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(order);
                PayPalHttpClient paypalHttpClient = client(paypalSetup);
                response = await paypalHttpClient.Execute(request);

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        //### Capturing an Order
        //Before capturing an order, order should be approved by the buyer using the approve link in create order response

        public async Task<PayPalHttp.HttpResponse> captureOrder(MyPaypalSetup paypalSetup)
        {
            // Construct a request object and set desired parameters
            // Replace ORDER-ID with the approved order id from create order
            OrdersCaptureRequest request = new OrdersCaptureRequest(paypalSetup.PayerApprovedOrderId);
            request.RequestBody(new OrderActionRequest());
            PayPalHttpClient paypalHttpClient = client(paypalSetup);
            PayPalHttp.HttpResponse response = await paypalHttpClient.Execute(request);
            return response;
        }

        public class MyPaypalSetup
        {
            /// <summary>
            /// Provide value sandbox for testing,  provide value live for real money
            /// </summary>
            public string Environment { get; set; }
            /// <summary>
            /// Client id as provided by Paypal on dashboard. Ensure you use correct value based on your environment selection
            /// Use sandbox accounts for sandbox testing
            /// </summary>
            public string ClientId { get; set; }
            /// <summary>
            /// Secret as provided by Paypal on dashboard. Ensure you use correct value based on your environment selection
            /// Use sandbox accounts for sandbox testing
            /// </summary>
            public string Secret { get; set; }

            /// <summary>
            /// This is the URL that you will pass to paypal which paypal will use to redirect payer back to your website.
            /// So essentially it is the same controller URL that you must pass
            /// </summary>
            public string RedirectUrl { get; set; }

            /// <summary>
            /// Once order is created on Paypal, it redirects control to your app with a URL that shows order details. Your website must take the payer to this page
            /// so the payer approved the payment. Store this URL in this property
            /// </summary>
            public string ApproveUrl { get; set; }

            /// <summary>
            /// When paypal redirects control to your website it provides a Approved Order ID which we then pass it back to paypal to execute the order.
            /// Store this approved order ID in this property
            /// </summary>
            public string PayerApprovedOrderId { get; set; }
        }
    }
}

